#include <windows.h>
#include <tchar.h>

int main() {
    HWND hwnd = FindWindow(_T("Shell_traywnd"), _T(""));
    if (IsWindowVisible(hwnd))
        SetWindowPos(hwnd,0,0,0,0,0,SWP_HIDEWINDOW);
    else
        SetWindowPos(hwnd,0,0,0,0,0,SWP_SHOWWINDOW);
    return 0;
}