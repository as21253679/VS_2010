#define	BUF_SZIE	64
#include <windows.h> 
#include "winsock.h"
#include <stdio.h>
#include<stdlib.h>
#include<iostream>
#include<string>
using namespace std;
void win(int,int);
void WDel();
void LDel();
int cwin=0,clost=0;
char ccwin[1],cclost[1];

int main(int argc, char* argv[])
{
	int me,you,w=0,l=0;
	WSADATA			wsd;			
	SOCKET			sHost;			
	SOCKADDR_IN		servAddr;		
	char			buf[BUF_SZIE], input[BUF_SZIE];	
	int				retVal;			
	int             nServAddlen;
	char ip[20];
	int	port;

	printf("�п�J�n�s�u��IP(��JEXIT���}�{��): ");
	scanf("%s",ip);
	if(ip[0]=='E'&&ip[1]=='X'&&ip[2]=='I'&&ip[3]=='T') {
		system("pause");
		return 0;
	}
	printf("�п�J�n�s�u��port number: ");
	scanf("%d",&port);

	if (WSAStartup(MAKEWORD(2,2), &wsd) != 0)
	{
		printf("WSAStartup failed!\n");
		return -1;
	}

	sHost = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);	
	if(INVALID_SOCKET == sHost)
	{
		printf("socket failed!\n");
		WSACleanup();//
		return  -1;
	}

	if (argc ==3)
	{
		servAddr.sin_family =AF_INET;
		servAddr.sin_addr.s_addr = inet_addr(argv[1]);
		servAddr.sin_port = htons((short) atoi(argv[2]));
		nServAddlen  = sizeof(servAddr);
	}
	else
	{
		servAddr.sin_family =AF_INET;
		servAddr.sin_addr.s_addr = inet_addr(ip);
		servAddr.sin_port = htons((short) port);
		nServAddlen  = sizeof(servAddr);
	}


	retVal=connect(sHost,(LPSOCKADDR)&servAddr, sizeof(servAddr));	
	if(SOCKET_ERROR == retVal)
	{
		printf("connect failed!\n");	
		closesocket(sHost);	//
		WSACleanup();		//
		return -1;
	}
	system("cls");
	printf("�w�}�l�s�u!\n");
	printf("�п�J��`:");
	scanf("%s",&buf);
	retVal = send(sHost, buf, strlen(buf), 0);
		if (SOCKET_ERROR == retVal)
		{
			printf("send failed!\n");
			closesocket(sHost);	//
			WSACleanup();		//
			return -1;
		}
	printf("���ԤT�Ө�\n");
	printf("1.�ŤM 2.���Y 3.��\n");
	printf("WIN�G%d  LOST�G%d\n\n",w,l);
	while(1)
	{
		ZeroMemory(buf, BUF_SZIE);
		printf("�ۤv:");
		cin>>input;
		me= atoi(input);
		retVal = send(sHost, input, strlen(input), 0);
		if (SOCKET_ERROR == retVal)
		{
			printf("send failed!\n");
			closesocket(sHost);	//
			WSACleanup();		//
			return -1;
		}
		printf("���ݹ��X�����\n");
		ZeroMemory(buf, BUF_SZIE);
		retVal = recv(sHost, buf, sizeof(buf), 0);

		if (SOCKET_ERROR == retVal)
		{
			printf("recv failed!\n");
			closesocket(sHost);	//
			WSACleanup();		//
			return -1;
		}
		you= atoi(buf);
		win(me,you);
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
end:
	closesocket(sHost);	
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
