#include <stdio.h>

#include <stdlib.h>
#include <WinSock2.h>
#define SERVER_PORT 23230
#define BUFFER_SIZE 1000
char cmdline[BUFFER_SIZE];
char *filename;
char *cmd;
/*
Server 等待連線
Protocol:
client -> server
server ->(send to client)->"i'm a file server"
*/
void getFileList(char *cmp ,char *buffer)
{
	HANDLE hFile;
	WIN32_FIND_DATA fd;
	//char buffer[MAX_BUFFER];
	char temp[50000];
	hFile=FindFirstFile(cmp,&fd);
	ZeroMemory(buffer,sizeof(buffer));
	do
	{
		if(fd.dwFileAttributes==0x10)	
			sprintf(temp,"[%s]\n",fd.cFileName);
		else
			sprintf(temp,"%s\n",fd.cFileName);
		strcat(buffer,temp);//串接buffer,temp,temp只會保留一行
	}while(FindNextFile(hFile,&fd));
	FindClose(hFile);
}

int sendfunc(SOCKADDR_IN *cilent_addr)//get
{
	int ret,actually_read_bytes;
	char buffer[BUFFER_SIZE];
	SOCKET client;
	FILE *in; 
	in = fopen(filename,"rb");
	if(!in)
	{
		return -3;
	}
	else //開檔成功 
	{
		client=socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
		if(client==INVALID_SOCKET)
		{
			puts("Getting data channel socket ID has failed");
			return -1;
		}
		ret=connect(client,(SOCKADDR*)cilent_addr,
			sizeof(*cilent_addr));
		if(ret==SOCKET_ERROR)
		{
			puts("Connection to data channel has failed");
			return -2;
		}
		while( !feof(in) )
		{
			actually_read_bytes = fread(buffer,sizeof(char),
				BUFFER_SIZE,in);
			//6 send to local_client, the data channel
			if( (ret = send(client,buffer,actually_read_bytes,
				0)) <= 0)
			{
				puts("Connection has been shutdown.");
				break;
			}
			printf("Sent %d bytes\n",ret);
		}
		fclose(in);	
		shutdown(client,SD_BOTH);
		closesocket(client);
	}//else
	return ret;
}
int recvfunc(SOCKADDR_IN *cilent_addr)//put
{
	int ret;
	SOCKET client;
	char buffer[BUFFER_SIZE];
	FILE *out; 
	client=socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	if(client==INVALID_SOCKET)
	{
		puts("Getting data channel socket ID has failed");
		return -1;
	}
	ret=connect(client,(SOCKADDR*)cilent_addr,
		sizeof(*cilent_addr));
	if(ret==SOCKET_ERROR)
	{
		puts("Connection to data cha");
		return -2;
	}
	out=fopen(filename,"wb");
	while(1)
	{
		ret=recv(client,buffer,BUFFER_SIZE,0);
		if(ret<=0)
		{
			puts("接收檔案失敗");
			break;
		}
		fwrite(buffer,1,ret,out);
	}
	fclose(out);	
	shutdown(client,SD_BOTH);
	closesocket(client);
	return 0;
}
int main(int argc,char *argv[])
{
	FILE *in;
	char path[1000];
	SOCKET server , client , local_client;
	SOCKADDR_IN server_addr , client_addr ;
	WSADATA wsd;
	char buffer[BUFFER_SIZE];
	int actually_read_bytes , ret , addr_size;
	short data_port;
	//1 startup winsocket environment
	if(WSAStartup(MAKEWORD(2,2),&wsd)!=0)
	{
		puts("Could not startup winsocket environment");
		return -1;
	}
	//2 get socket ID
	server = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	if(server == INVALID_SOCKET)
	{
		puts("Could not get socket handle.");
		WSACleanup();
		return -1;
	}
	//3 bind
	server_addr.sin_addr.s_addr = INADDR_ANY;
	server_addr.sin_family = AF_INET;
	server_addr.sin_port = htons(SERVER_PORT);
	ret = bind(server,(SOCKADDR*)&server_addr,sizeof(server_addr));
	if(ret == SOCKET_ERROR)
	{
		puts("Could not bind socket address info.");
		WSACleanup();
		closesocket(server);
		return -1;
	}
	//4 listen
	ret = listen(server,10); 
	if(ret == SOCKET_ERROR)
	{
		puts("Could not start to listen.");
		WSACleanup();
		closesocket(server);
		return -1;	
	}
	puts("Server is up to work");
	printf("Server IP:%s\n",inet_ntoa(server_addr.sin_addr ));
	printf("Server Port:%d\n",ntohs(server_addr.sin_port));

	//5 accept
	addr_size = sizeof(client_addr);
	while(1)
	{
		client = accept(server,(SOCKADDR*)&client_addr,&addr_size);
		data_port = ntohs(client_addr.sin_port) + 1;
		if(client == INVALID_SOCKET)
		{
			puts("Accepting failed.");
			WSACleanup();
			closesocket(server);
			return -1;	
		}	
		puts("Accepted a client.");
		printf("Client IP:%s\n",inet_ntoa(client_addr.sin_addr ));
		printf("Client Port:%d\n",ntohs(client_addr.sin_port));

		/*Send a greeting message to client.*/
		ret = send(client,"I'm a file server",strlen("I'm a file server")+1,0);
		if( ret <= 0)
		{
			puts("Connection has been shutdown.");
			return -1;
		}

		/*6 Open file loop (Create a client connect to client's 
		data channel
		*/

		while(1)
		{
			printf(">");
			ret = recv(client,buffer,sizeof(buffer),0);
			if(ret <= 0)
			{
				puts("Connection has been shutdown.");
				break;
			}
			buffer[ret] = '\0'; // 字串結束'\0'字元(重要) 
			// ret:接收到的位元組數
			puts(buffer);		
			strcpy(cmdline,buffer);
			cmd=strtok(cmdline," ");
			filename=strtok(NULL," ");
			client_addr.sin_port = htons(data_port);

			if( strcmp(buffer,"exit") == 0 )
			{
				ret = send(client,"ByeBye",strlen("ByeBye")+1,0);
				shutdown(client,SD_BOTH);
				closesocket(client);
				break;
			}
			else if(strcmp(cmd,"get") == 0)
			{
				if(!filename)
				{
					send(client,"Usage:get filename",
						strlen("Usage:get filename")+1,0);
					continue;
				}
				ret=sendfunc(&client_addr);
				if(ret==-3)//-3=Could not find file
				{
					ret = send(client,"The file is not existed",
						strlen("The file is not existed")+1,
						0); //response to client
					if(ret <= 0)
					{
						puts("Connection has been disconnected.");
						break;
					}
				}
				else if(ret == 0)
				{
					send(client,"The in struction is invalid",
						strlen("The instruetion is invalid")+1,0);

				}
			}
			else if(strcmp(cmd,"put") == 0)
			{
				if(recvfunc(&client_addr)==0)
					send(client,"Upload the file has done",
					strlen("Upload the file has done")+1,0);
			}
			else if(strcmp(cmd,"dir") == 0)
			{
				cmd=strtok(NULL," ");
				if(!cmd)
					cmd="*";
				getFileList(cmd,buffer);
				ret=send(client,buffer,sizeof(buffer),0);
				if(ret <= 0)
				{
					puts("失敗");
					break;
				}
			}
			else if(strcmp(cmd,"cd") == 0)
			{
				if(SetCurrentDirectory(filename))
				{
					GetCurrentDirectory(sizeof(path),path);
					ret=send(client,path,sizeof(path),0);
					if(ret <= 0)
					{
						puts("失敗");
						break;
					}
				}
			}
			else
			{
				ret=send(client,"The in struction is invalid",
					strlen("The instruetion is invalid")+1,0);
				continue;
			}
		}//receiving loop
	}//accepting loop
	getchar();
	return 0;
}	

