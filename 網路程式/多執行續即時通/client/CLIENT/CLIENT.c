#define	BUF_SIZE	256
#include <winsock2.h>
#include <stdio.h>
#include <string.h>
#include <Windows.h>
#include <process.h>
void recvThread(void *arg);
int main(int argc,char* argv[])
{
	WSADATA			wsd;			
	SOCKET			sHost;			
	SOCKADDR_IN		servAddr;		
	char			buf[BUF_SIZE];	
	int				retVal,connFlag;			
	int             nServAddien;
	HANDLE          rHandler;

	if (WSAStartup(MAKEWORD(2,2), &wsd) != 0)
	{
		printf("WSAStartup failed!\n");
		getchar();
		return -1;
	}

	connFlag=0;
	servAddr.sin_family =AF_INET;
	servAddr.sin_addr.s_addr = inet_addr("127.0.0.1");
	servAddr.sin_port = htons((short) 2323);
	nServAddien  = sizeof(servAddr);

	ZeroMemory(buf, BUF_SIZE);
	printf("請輸入要傳送的訊息(離開請輸入END或E):\n");
	fflush(stdin);

	while(1)
	{
		if(connFlag==0)
		{
			printf("#");
			sHost = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
			if(INVALID_SOCKET == sHost)
			{
				printf("socket failed!\n");
				continue;
			}
		
	fgets(buf,BUF_SIZE,stdin);
	if(strcmp(buf,"quit\n")==0)
	{
		printf("Terminating Program!!\nPress Any key to quit!\n");
		getchar();
		break;
	}
		servAddr.sin_addr.s_addr = inet_addr(buf);
	    servAddr.sin_port = htons((short) 2323);
		retVal =connect(sHost,(LPSOCKADDR)&servAddr,sizeof(servAddr));
		if(SOCKET_ERROR == retVal)
		{
			printf("connect failed!\n");
			continue;
		}
	rHandler=(HANDLE) _beginthread(recvThread,0,(void *) sHost);
	connFlag=1;
	printf("連線成功!!\n");
	}
	printf(">");
	ZeroMemory(buf,sizeof(buf));
	fflush(stdin);
	fgets(buf,BUF_SIZE,stdin);
	if((strcmp(buf,"END\n")==0) || (strcmp(buf,"E\n")==0))
	{
		connFlag=0;
		shutdown(sHost,SD_SEND);
		WaitForSingleObject(rHandler,INFINITE);
		closesocket(sHost);
		continue;
	}
	retVal=send(sHost,buf,strlen(buf),0);
	if(SOCKET_ERROR == retVal)
	{
		printf("send failed!\n");
		break;
	}
	}
	return 0;
}
void recvThread(void *arg)
{
char buf[BUF_SIZE];
SOCKET sockfd;
int numbytes;
sockfd = (SOCKET) arg;
while(1)
{
	numbytes = recv(sockfd,buf,sizeof(buf),0);
	if(numbytes <= 0)
	{
	printf("伺服器關閉連線!!\n");
	break;
	}
	buf[numbytes]='\0';
	printf("\n伺服器:%s\n>",buf);
}
}