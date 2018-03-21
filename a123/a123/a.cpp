#include<iostream>
#include <time.h>
using namespace std;

int success = 0;
int r=0,c=0;
int ranarray[100][100];
int visit(int i, int j) { 
    ranarray[i][j] = 5; 

    if(i == r-1 && j == c-1)
        success = 1; 

    if(success != 1 && ranarray[i][j+1] == 1) visit(i, j+1); 
    if(success != 1 && ranarray[i+1][j] == 1) visit(i+1, j); 
    if(success != 1 && ranarray[i][j-1] == 1) visit(i, j-1); 
    if(success != 1 && ranarray[i-1][j] == 1) visit(i-1, j); 

    if(success != 1) 
        ranarray[i][j] = 8; 
    
    return success; 
}  

int main()
{
	
	int e=0;
	int ran,ran2;
	
	int a[100][100];
	srand(time(NULL));

	while(1)
	{
		e=0;
		success=0;
		
		cout<<"請輸入row:";
		cin>>r;
		cout<<"請輸入column:";
		cin>>c;
		cout<<"請輸入enemy:";
		cin>>e;
		cout<<endl;

		redo:
		for(int i=0;i<100;i++)
		{
			for(int j=0;j<100;j++)
			{
				ranarray[i][j]=9;
				a[i][j]=0;
			}
		}
		for(int i=0;i<r;i++)
		{
			for(int j=0;j<c;j++)
			{
				ranarray[i][j]=1;
			}
		}

		for(int i=0;i<e;i++)
		{
			re:
			ran=rand()%r;
			ran2=rand()%c;
			if(ranarray[ran][ran2]==3)
				goto re;
			else
			{
				a[ran][ran2]=1;
				if(ran==0 && ran2==0)
				{
					ranarray[ran+1][ran2]=2;
					ranarray[ran][ran2+1]=2;
					ranarray[ran+1][ran2+1]=2;
				}
				else if(ran==0)
				{
					ranarray[ran][ran2-1]=2;
					ranarray[ran][ran2+1]=2;
					ranarray[ran+1][ran2+1]=2;
					ranarray[ran+1][ran2]=2;
					ranarray[ran+1][ran2-1]=2;
				}
				else if(ran2==0)
				{
					ranarray[ran-1][ran2]=2;
					ranarray[ran+1][ran2]=2;
					ranarray[ran+1][ran2+1]=2;
					ranarray[ran][ran2+1]=2;
					ranarray[ran-1][ran2+1]=2;
				}
				else
				{
					ranarray[ran-1][ran2-1]=2;
					ranarray[ran-1][ran2]=2;
					ranarray[ran-1][ran2+1]=2;
					ranarray[ran][ran2-1]=2;
					ranarray[ran][ran2]=2;
					ranarray[ran][ran2+1]=2;
					ranarray[ran+1][ran2-1]=2;
					ranarray[ran+1][ran2]=2;
					ranarray[ran+1][ran2+1]=2;
				}
			}
			for(int z=0;z<r;z++)
			{
				for(int j=0;j<c;j++)
				{
					if(a[z][j]==1)
					{
						ranarray[z][j]=3;
					}
				}
			}
		}
		cout<<"敵人位置"<<endl;
		for(int i=0;i<r;i++)
		{
			for(int j=0;j<c;j++)
			{
				if(ranarray[i][j]==1)
					cout<<'O'<<" ";
				else if(ranarray[i][j]==2)
					cout<<'X'<<" ";
				else if(ranarray[i][j]==3)
					cout<<'@'<<" ";
				else
					cout<<ranarray[i][j]<<" ";
			}
			cout<<endl;
		}
		cout<<endl;

		visit(0,0);
		cout<<"路徑"<<endl;
		for(int i=0;i<r;i++)
		{
			for(int j=0;j<c;j++)
			{
				if(ranarray[i][j]==1)
					cout<<'O'<<" ";
				else if(ranarray[i][j]==2)
					cout<<'X'<<" ";
				else if(ranarray[i][j]==3)
					cout<<'@'<<" ";
				else if(ranarray[i][j]==5)
					cout<<"^"<<" ";
				else 
					cout<<ranarray[i][j]<<" ";
			}
			cout<<endl;
		}
		cout<<endl;
		if(success==0)
		{
			cout<<"//////失敗/////"<<endl<<endl<<endl;
			goto redo;
		}
		cout<<"/////成功/////"<<endl<<endl<<endl;
	}
}