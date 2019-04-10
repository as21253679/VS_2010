#include <stdio.h>
#include <stdlib.h>
#include <errno.h>
#include <string.h>
#include <process.h>
#include <winsock2.h>
#include <windows.h>
#define BUF_SIZE 256
void recvThread(void *arg);
void sendThread(void *arg);
int connFlag;
int main(){
	int sockfd; // server listenning socket! 
	int	new_fd; // server accepted socket!
	int numbytes;
	struct sockaddr_in my_addr;
	struct sockaddr_in their_addr;
	int sin_size;
	char buf[BUF_SIZE];
	char *clientIP;
	unsigned short clientPort;
	HANDLE Handlers[2];
	WSADATA wsd;

if (WSAStartup(MAKEWORD(2,2), &wsd) != 0)
{
	printf("WSAStartup failed!\n");
	return -1;
}
if( (sockfd = socket(AF_INET, SOCK_STREAM, 0))==-1 )
{
	perror("socket");
	WSACleanup();//
	exit(1);
}
my_addr.sin_family =AF_INET;
my_addr.sin_port = htons(2323);
my_addr.sin_addr.s_addr = htonl(INADDR_ANY);
ZeroMemory(&(my_addr.sin_zero),8);

if(bind(sockfd,(struct sockaddr*)&my_addr, sizeof(struct sockaddr))==-1)
{
	perror("bind");
	closesocket(sockfd);
	WSACleanup();
	exit(1);
}
if(listen(sockfd,10)==-1)
{
	perror("listen");
	closesocket(sockfd);
	WSACleanup();
	exit(1);
}
sin_size = sizeof(struct sockaddr_in);
while(1)
{
	connFlag=0;
	printf("伺服器等待連線中!!\n");
	if((new_fd = accept(sockfd,(struct sockaddr*)&their_addr,&sin_size))==-1)
	{
		perror("accept");
		closesocket(sockfd);
		WSACleanup();
		exit(1);
	}
	connFlag=1;
	printf("用戶端已開始連線!!\n");
	clientIP=inet_ntoa(their_addr.sin_addr);
	clientPort=(unsigned short)ntohs(their_addr.sin_port);
	printf("Client IP:%s\n",clientIP);
	printf("Client Port:%d\n",clientPort);
	Handlers[0]=(HANDLE) _beginthread(recvThread,0,(void *)new_fd);
	Handlers[1]=(HANDLE) _beginthread(sendThread,0,(void *)new_fd);
	
	/*WaitForSingleObject(Handlers[0],INFINITE);

	closesocket(new_fd);*/
}
closesocket(sockfd);
WSACleanup();
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
	if( numbytes <= 0)
	{
		printf("recv:用戶端關閉連線!!\n");
		break;
	}
	buf[numbytes] = '\0';
	printf("\n客戶端:%s\n>",buf);
	if((strcmp(buf,"END\n")==0) || (strcmp(buf,"E\n")==0))
	break;
}
connFlag=0;
shutdown(sockfd,SD_BOTH);
}
void sendThread(void *arg)
{
char buf[BUF_SIZE];
SOCKET sockfd;
int numbytes;
sockfd = (SOCKET) arg;
while(1)
{
	ZeroMemory(buf,sizeof(buf));
	printf(">");
	fgets(buf,BUF_SIZE,stdin);
	numbytes = send(sockfd,buf,strlen(buf),0);
	if(numbytes == -1)
	{
	printf("send:用戶端關閉連線!!\n");
	break;
	}
}
}