#include <stdio.h>
#include <stdlib.h>
#include <winSock.h>
#include <string>
#include<iostream>
using namespace std;
void win(int ,int );
void WDel();
void LDel();
int cwin=0,clost=0;
char ccwin[1],cclost[1];

int main()
{
	int me,you,w=0,l=0;
	int sockfd; // server listenning socket! 
	int	new_fd; // server accepted socket!
	int numbytes;
	struct sockaddr_in my_addr;
	struct sockaddr_in their_addr;
	int sin_size;
	char buf[10],input[10];
	WSADATA wsd;
	int				retVal;	

	if (WSAStartup(MAKEWORD(2,2), &wsd) != 0)
	{
		printf("WSAStartup failed!\n");
		return -1;
	}

	if( (sockfd = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP))==INVALID_SOCKET )
	{
		printf("socket failed!\n");
		WSACleanup();//
		return  -1;
	}
	my_addr.sin_family =AF_INET;
	my_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	my_addr.sin_port = htons((short) 2323);
	sin_size  = sizeof(my_addr);
	// binding 
	if(bind(sockfd,(struct sockaddr *) &my_addr, sin_size) == -1)
	{
		printf("binding failed!\n");
		closesocket(sockfd);
		WSACleanup();//
		return  -1;
	}
	// Starting listening
	if(listen(sockfd, 10) == -1)
	{
		printf("binding failed!\n");
		closesocket(sockfd);
		WSACleanup();//
		return  -1;
	}

	// Waiting for clients to come-in
	printf("等待玩家連線中!\n");
	if ((new_fd=accept(sockfd, (struct sockaddr *) &their_addr, &sin_size)) == -1)
	{
		printf("accepting failed!\n");
		closesocket(sockfd);
		WSACleanup();//
		return  -1;
	}
	system("cls");
	printf("玩家已開始連線!\n");
	printf("賭注為:");
	numbytes=recv(new_fd, buf,sizeof(buf),0);
	buf[numbytes]='\0';
	printf("%s\n",buf);
	printf("五戰三勝制\n");
	printf("1.剪刀 2.石頭 3.布\n");
	printf("WIN：%d  LOST：%d\n\n",w,l);

	//server first make a receive!
	while(1)
	{
		printf("自己:");
		scanf("%s",input);
		printf("等待對方出拳～～\n");
		me = atoi(input);
		numbytes=recv(new_fd, buf,sizeof(buf),0);
		if (SOCKET_ERROR == numbytes)
		{
			printf("recv failed!\n");
			closesocket(new_fd);	//
			WSACleanup();		//
			return -1;
		}
		buf[numbytes]='\0';
		you= atoi(buf);
		numbytes=send(new_fd, input, strlen(input),0);
		if (SOCKET_ERROR == numbytes)
		{
			printf("send failed!\n");
			closesocket(new_fd);	//
			WSACleanup();		//
			return -1;
		}
		win(me,you);
		// server send back what it received.

		if(cwin==3)
		{
			printf("你已經取得了三勝,這場比賽你贏了\n");
			goto end;
		}
		else if(clost==3)
		{
			printf("你已經輸了三次,這場比賽你輸了\n");
			goto end;
		}
	}
	printf("對方已離開! \n\n");
	//system("pause");
end:
	closesocket(new_fd);
	closesocket(sockfd);
	WSACleanup();
	system("pause");
	return 0;
}

void win(int me,int you)
{
	string a,b;
	switch (me)
	{
	case 1: a="剪刀";
		break;
	case 2: a="石頭";
		break;
	case 3: a="布";
		break;
	}
	switch (you)
	{
	case 1: b="剪刀";
		break;
	case 2: b="石頭";
		break;
	case 3: b="布";
		break;
	}
	if(me==you)
		cout<<"你出"<<a<<",對方出"<<b<<",這局平手"<<endl<<endl;
	else if(me==1&&you==3)
	{
		cout<<"你出"<<a<<",對方出"<<b<<",你贏了"<<endl<<endl;
		cwin++;
		WDel();
	}
	else if(me==2&&you==1)
	{
		cout<<"你出"<<a<<",對方出"<<b<<",你贏了"<<endl<<endl;
		cwin++;
		WDel();
	}
	else if(me==3&&you==2)
	{
		cout<<"你出"<<a<<",對方出"<<b<<",你贏了"<<endl<<endl;
		cwin++;
		WDel();
	}
	else if(me==1&&you==2)
	{
		cout<<"你出"<<a<<",對方出"<<b<<",你輸了"<<endl<<endl;
		clost++;
		LDel();
	}
	else if(me==2&&you==3)
	{
		cout<<"你出"<<a<<",對方出"<<b<<",你輸了"<<endl<<endl;
		clost++;
		LDel();
	}
	else if(me==3&&you==1)
	{
		cout<<"你出"<<a<<",對方出"<<b<<",你輸了"<<endl<<endl;
		clost++;
		LDel();
	}
}

void WDel() 
{ 
	HANDLE hOutput; 
	CONSOLE_SCREEN_BUFFER_INFO sbi; 
	DWORD len, nw; 
	COORD cd = {5, 4}; 
	hOutput = GetStdHandle(STD_OUTPUT_HANDLE); 
	GetConsoleScreenBufferInfo(hOutput, &sbi); 
	sprintf(ccwin,"%d",cwin);
	FillConsoleOutputCharacter(hOutput,ccwin[0],1,cd,&nw); 
}

void LDel() 
{ 
	HANDLE hOutput; 
	CONSOLE_SCREEN_BUFFER_INFO sbi; 
	DWORD len, nw; 
	COORD cd = {14, 4}; 
	hOutput = GetStdHandle(STD_OUTPUT_HANDLE); 
	GetConsoleScreenBufferInfo(hOutput, &sbi); 
	sprintf(cclost,"%d",clost);
	FillConsoleOutputCharacter(hOutput,cclost[0],1,cd,&nw); 
}

