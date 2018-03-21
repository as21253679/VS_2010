#include<windows.h>

int main(){ 
	//system("start notepad.exe"); 
	Sleep(5000); 
	for(int i=0;i<2000;i++)
	{
		mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
		Sleep(1000); 
		mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
		Sleep(1000); 
	}
	/*for(int i=0;i<=25;i++){ 
	keybd_event(65+i,0,0,0);
	keybd_event(65+i,0,KEYEVENTF_KEYUP,0); 
	Sleep(200); 
	} */
} 