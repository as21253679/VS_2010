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
	printf("���ݪ��a�s�u��!\n");
	if ((new_fd=accept(sockfd, (struct sockaddr *) &their_addr, &sin_size)) == -1)
	{
		printf("accepting failed!\n");
		closesocket(sockfd);
		WSACleanup();//
		return  -1;
	}
	system("cls");
	printf("���a�w�}�l�s�u!\n");
	printf("��`��:");
	numbytes=recv(new_fd, buf,sizeof(buf),0);
	buf[numbytes]='\0';
	printf("%s\n",buf);
	printf("���ԤT�Ө�\n");
	printf("1.�ŤM 2.���Y 3.��\n");
	printf("WIN�G%d  LOST�G%d\n\n",w,l);

	//server first make a receive!
	while(1)
	{
		printf("�ۤv:");
		scanf("%s",input);
		printf("���ݹ��X�����\n");
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
			printf("�A�w�g���o�F�T��,�o�����ɧAĹ�F\n");
			goto end;
		}
		else if(clost==3)
		{
			printf("�A�w�g��F�T��,�o�����ɧA��F\n");
			goto end;
		}
	}
	printf("���w���}! \n\n");
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
	case 1: a="�ŤM";
		break;
	case 2: a="���Y";
		break;
	case 3: a="��";
		break;
	}
	switch (you)
	{
	case 1: b="�ŤM";
		break;
	case 2: b="���Y";
		break;
	case 3: b="��";
		break;
	}
	if(me==you)
		cout<<"�A�X"<<a<<",���X"<<b<<",�o������"<<endl<<endl;
	else if(me==1&&you==3)
	{
		cout<<"�A�X"<<a<<",���X"<<b<<",�AĹ�F"<<endl<<endl;
		cwin++;
		WDel();
	}
	else if(me==2&&you==1)
	{
		cout<<"�A�X"<<a<<",���X"<<b<<",�AĹ�F"<<endl<<endl;
		cwin++;
		WDel();
	}
	else if(me==3&&you==2)
	{
		cout<<"�A�X"<<a<<",���X"<<b<<",�AĹ�F"<<endl<<endl;
		cwin++;
		WDel();
	}
	else if(me==1&&you==2)
	{
		cout<<"�A�X"<<a<<",���X"<<b<<",�A��F"<<endl<<endl;
		clost++;
		LDel();
	}
	else if(me==2&&you==3)
	{
		cout<<"�A�X"<<a<<",���X"<<b<<",�A��F"<<endl<<endl;
		clost++;
		LDel();
	}
	else if(me==3&&you==1)
	{
		cout<<"�A�X"<<a<<",���X"<<b<<",�A��F"<<endl<<endl;
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

