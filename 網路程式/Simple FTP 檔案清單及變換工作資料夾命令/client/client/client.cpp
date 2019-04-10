#include <stdio.h>
#include <stdlib.h>
#include <WinSock2.h>
#include<Windows.h>
#include <string.h>
#include <process.h>
#define BUFFER_SIZE 1000
#define SERVER_IP "127.0.0.1"
#define SERVER_PORT 23230
int exit_flag;
///12/29
//get filename
//put filename
//新增
char *filename;
char cmdline[BUFFER_SIZE];
char *cmd;

void data_channel(void *param)
{
	//ver. 2
	//
	int ret , data_len;
	int recv_bytes;
	int inet_len;
	char buffer[BUFFER_SIZE];
	FILE  *in,*out;
	SOCKET local_serv;
	SOCKADDR_IN *my_addr = (SOCKADDR_IN*)param;
	SOCKET datafd;
	SOCKADDR_IN data_addr;

	//7 get server port socket ID
	local_serv = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	if(local_serv == INVALID_SOCKET)
	{
		puts("Could not get local server socket handle");
		WSACleanup();
		return;
	} 	
	//8 prepare server port address structure
	//local server port = local port+1
	my_addr->sin_port = htons ( ntohs(my_addr->sin_port) +1);
	//9 binding local server to port of control channel + 1
	ret = bind(local_serv,(SOCKADDR*)my_addr,sizeof(*my_addr));
	if(ret == SOCKET_ERROR)
	{
		puts("Could not bind socket address info.");
		return;
	}
	//10 listen
	ret = listen(local_serv,1);
	if(ret == SOCKET_ERROR)
	{
		puts("listening failed.");
		return;
	}

	data_len = sizeof(data_addr);
	printf("\nData channel enabled!\n>");
	while(1)
	{
		datafd = accept(local_serv,(SOCKADDR*)&data_addr
			,&data_len);
		if(datafd == INVALID_SOCKET)
		{
			puts("Accepting from server failed on data channel"); 
			return;
		}

		if(strcmp(cmd,"exit")==0)
		{
			break;
		}
		else if(strcmp(cmd,"get")==0)
		{
			out = fopen (filename,"wb");
			if(!out)
			{
				puts("Could not write the file");
				return;
			}
			while(1)
			{
				recv_bytes = recv(datafd,buffer,sizeof(buffer),0);
				if(recv_bytes <= 0)
				{
					puts("Connection has been shutdown.");
					break;
				}
				//printf("Recv bytes:%d\n",recv_bytes);
				fwrite(buffer,sizeof(char) , recv_bytes, out);
			}
			fclose(out);
			printf("Received %s successfully\n>",filename);
			closesocket(datafd);
		}
		else if(strcmp(cmd,"put")==0)
		{
			in=fopen(filename,"rb");
			if(!in)
			{
				puts("Could not write the file");
				return;
			}
			while(!feof(in))
			{
				recv_bytes = fread(buffer,sizeof(char),
					BUFFER_SIZE,in);

				ret=send(datafd,buffer,recv_bytes,0);
				if(recv_bytes <= 0)
				{
					puts("傳送檔案失敗");
					break;
				}
			}
		}//put
		else if(strcmp(cmd,"dir")==0)
		{
			recv_bytes = recv(datafd,buffer,sizeof(buffer),0);
			if(recv_bytes <= 0)
			{
				break;
			}
			puts(buffer);
		}
		else if(strcmp(cmd,"cd")==0)
		{
			recv_bytes = recv(datafd,buffer,sizeof(buffer),0);
			if(recv_bytes <= 0)
			{
				break;
			}
			puts(buffer);
		}
		shutdown(datafd,SD_BOTH);
		closesocket(datafd);
	}//accept loop
}
//ver. 1
/*void data_channel(void *param) //socket
{
int ret , data_len;
int recv_bytes;
FILE  *out;
char buffer[BUFFER_SIZE];
SOCKET local_serv = (SOCKET)param;//強迫轉型成SOCKET型態
SOCKET datafd;
SOCKADDR_IN data_addr;
data_len = sizeof(data_addr);
puts("Data channel enabled!");
while(1)
{
datafd = accept(local_serv,(SOCKADDR*)&data_addr
,&data_len);
if(datafd == INVALID_SOCKET)
{
puts("Accepting from server failed on data channel"); 
return;
}
out = fopen (filename,"wb");
if(!out)
{
puts("Could not write the file");
return;
}
while(1)
{
recv_bytes = recv(datafd,buffer,sizeof(buffer),0);
if(recv_bytes <= 0)
{
puts("Connection has been shutdown.");
break;
}
printf("Recv bytes:%d\n",recv_bytes);
fwrite(buffer,sizeof(char) , recv_bytes, out);
}
fclose(out);
printf("檔案複製結束\n");
closesocket(datafd);
}//accept loop
}*/

int main(int argc,char *argv[])
{
	FILE  *in,*out;
	char buffer[BUFFER_SIZE];
	char cmdlinetemp[BUFFER_SIZE];
	int recv_bytes ,ret ,inet_len;
	WSADATA wsd;
	SOCKET host , local_serv;
	SOCKADDR_IN host_addr , my_addr;
	inet_len = sizeof(my_addr);
	//1 startup winsocket environment
	if(WSAStartup(MAKEWORD(2,2),&wsd) != 0)
	{
		puts("Could not startup winsocket environment");
		return -1;
	}
	//2 get socket ID
	host = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	if(host == INVALID_SOCKET)
	{
		puts("Could not get socket handle");
		WSACleanup();
		return -1;
	}

	//3 specify server address
	host_addr.sin_addr.s_addr = inet_addr(SERVER_IP);
	host_addr.sin_family = AF_INET;
	host_addr.sin_port = htons(SERVER_PORT);

	//4 connect to server
	ret = connect(host,(SOCKADDR*)&host_addr,sizeof(host_addr));
	if(ret == SOCKET_ERROR)
	{
		puts("Could not connect to server.");
		closesocket(host);
		WSACleanup();
		return -1;
	}
	//5. greeting message
	ret = recv(host,buffer,sizeof(buffer),0);
	if(ret <= 0)
	{
		puts("Connection has been disconnected.");
		return -1;
	}
	//buffer[ret] = 0;
	printf("Server:%s\n",buffer);

	//6 start client side server port for data channel
	ret = getsockname(host,(SOCKADDR*)&my_addr,&inet_len);
	if(ret == SOCKET_ERROR)
	{
		puts("getsockname failed!");
		return -1;
	}
	printf("IP:%s\nPort:%d\n",inet_ntoa(my_addr.sin_addr),
		ntohs(my_addr.sin_port));
	//11 start data channel thread

	_beginthread(data_channel,0,(void*)&my_addr);

	while(!exit_flag) //command loop via control channel
	{
		printf(">");
		fgets(cmdline,sizeof(cmdline),stdin);
		cmdline[strlen(cmdline)-1] = '\0'; 
		if(cmdline[0]=='\0') 
			continue;
		//cmd -> get/put ...etc
		//string token ...abc.exe(token=='.')
		//abc.exe -> abc\0exe\0
		strcpy(cmdlinetemp,cmdline);

		cmd=strtok(cmdlinetemp," ");
		if(!cmd) continue;
		filename=strtok(NULL," ");

		if( strcmp (cmd,"exit") == 0)
		{
			exit_flag = 1;
			continue;//省略後面動作
		}
		else if(strcmp (cmd,"put") == 0)
		{
			in=fopen(filename,"rb");
			if(!in)
			{
				printf("Could not find the file %s \n",
					filename);
				continue;
			}
		}

		ret = send(host,cmdline,strlen(cmdline),0);
		if(ret <= 0)
		{
			puts("Connection has been disconnected.");
			return -1;
		}
		ret = recv(host,buffer,sizeof(buffer),0);
		if(ret <= 0)
		{
			puts("Connection has been disconnected.");
			return -1;
		}
		buffer[ret] = '\0';
		printf("Server:%s\n",buffer); //server response
	}//command loop
	closesocket(host);
	WSACleanup();
	system("pause");
	return 0;
}