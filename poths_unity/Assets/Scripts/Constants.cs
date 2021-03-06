public static class Constants
{
	public const int BUTTONHIGH =	1;
	public const int BUTTONMID =	2;
	public const int BUTTONLOW =	4;
	public const int BUTTONRIGHT =	8;
	public const int BUTTONLEFT =	16;
	public const int BUTTONUP =		32;
	public const int BUTTONDOWN =	64;
	public const int BUTTONBLOCK =	128;
	
	// diagonals
	public const int BUTTONDOWNRIGHT =	(BUTTONDOWN | BUTTONRIGHT);
	public const int BUTTONDOWNLEFT =	(BUTTONDOWN | BUTTONLEFT);
	public const int BUTTONUPRIGHT =	(BUTTONUP | BUTTONRIGHT);
	public const int BUTTONUPLEFT =		(BUTTONUP | BUTTONLEFT);
}