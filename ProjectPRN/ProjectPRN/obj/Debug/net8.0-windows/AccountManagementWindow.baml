   M S B A M L   `   `   `  ����E  AProjectPRN, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:ProjectPRN
ProjectPRN  5   	   (    "ProjectPRN.AccountManagementWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:ProjectPRN     5   	     � Title$  Quản Lý Tài Khoản��5   	   $	��700��6#   $
��1200��60    � WindowStartupLocation$ CenterScreen=�5	   	   $��Segoe UI)�5
   	    � WindowState$ 	Maximized=�5   	    � 	Resources 5       �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius	 �� Triggers
 P� Property P� Value   DeleteButton �� BasedOn ��Name d� 	IsChecked �� Path �� Mode �� RelativeSource � ColumnDefinitions �� SelectionBoxItem �� SelectionBoxItemTemplate �� IsDropDownOpen � AllowsTransparency J� SnapsToDevicePixels � ActualWidth �� MaxDropDownHeight ��Name K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation   DirectionalNavigation(  "System.Windows.Controls.DatePicker&   System.Windows.Controls.DataGrid=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%�  '��       &	  �    7��'�� �    'G� ?    '�� �    '  �    '  �    '  �    �� 5   
   " ��6   .�5      ��  � ��$ 15,10q�6(   �� 5       � ��$ 5q�6'   �� 5       � �� �#1E88E56+   �� 5       � ��
 �����6+   �� 5       � ��$ 0q�60   �� 5       � ��$ SemiBold"�6+   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5      #��z��5      $ 4��5      ._�5      �� $��Center=�6/   $��Center=�6L   5      	 5      P� 5      
 �@J�IsMouseOver$
 True��6=   . �5   "   ��  � �� �#1976D26?   5       P� 5!      
 � ��$
 True��6;   . �5"   "   ��  � �� �#1565C06?   5#      5%      5'      5(      �� 5*   
   " ��6%   8   .�5+      ��  � �� �#E539356+   �� 5,       � �� 5-      �� 5.      " ��6&   .R�5/      �� #��z��6!   #��z��50      #��z��51      $ 4��52      ._�53      �� $��Center=�6/   $��Center=�6L   54      	 55      P� 56      
 �@J�IsMouseOver$
 True��6=   . �57   "   ��  � �� �#D32F2F6?   58      P� 59      
 � ��$
 True��6;   . �5:   "   ��  � �� �#C628286?   5;      5=      5?      5@      �� 5B   
   " ��6   .�5C      ��  � ��$
 10,8q�6(   �� 5D       � ��$	 0,5q�6'   �� 5E       � ��$ 1q�60   �� 5F       � �� �#BDBDBD6,   �� 5G       � �� 5H      �� 5I      " ��6&   .R�5J      �� #��z��6!   #��z��5K      #��z��5L      $ 4��5M      ._�5N      �� $ PART_ContentHost��6+   5O      5P      5R      5S      �� 5U   
   " G�6   .�5V      ��  � ��$
 10,8q�6(   �� 5W       � ��$	 0,5q�6'   �� 5X       � ��$ 1q�60   �� 5Y       � �� �#BDBDBD6,   �� 5Z       � �� 5[      �� 5\      " G�6&   .R�5]      �� #��z��6!   #��z��5^      #��z��5_      $ 4��5`      ._�5a      �� $ PART_ContentHost��6+   5b      5c      5e      5f      �� 5h   
   " ��6   .�5i      ��  � ��$
 10,8q�6(   �� 5j       � ��$ 30��6'   �� 5k       � ��$	 0,5q�6'   �� 5l       � ��$ 1q�60   �� 5m       � �� �#BDBDBD6,   �� 5n       � �� 5o      �� 5p      " ��6&   .R�5q      � .+�5r      d� $��ToggleButton��6+   #��z��5s   '   #��z��5t   '   #��z��5u   '   $|�false��5v   '    5w   '   �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+��5x   "   �� 5y   &   " d�66   .R�5z   *   �� #��z��61   #��z��5{   -   #��z��5|   -   $ 4��5}   -   ._�5~   .   �  5   2   �� 5�   6   $��* �6G   �� 5�   6   $��20 �6G   .+�5�   2   �� $��0��6C   $
��Left=�5�   >   $��3q�5�   >   $��Center=�5�   >   F� 5�   2   $��1��67   $��Center=�5�   2   $��Center=�5�   2   "����        � 	=  	= � z     	���#4445�   2   5�   /   5�   +   5�   '   5�      �� 5�      $��ContentSite��6/   #��z 5�   *   #��z 5�   *   #��z��5�   *   $��	10,3,30,3q�5�   *   $��Center=�5�   *   $
��Left=�5�   *   � 5�      $��Popup��6$   $��Bottom=�5�      #��z 5�      $
 True��5�      $|�False��5�      $��Slide=�5�      .��5�   "   � $��DropDown��6'   $
 True��5�   "   #��z 5�   "   #��z 5�   "   .+�5�   &   �� $ DropDownBorder��6-   
�������$��1q�5�   (   ���#BDBDBD$ 0,0,4,4��5�   (   �� 5�   &   $��4,6,4,6q�63   $
 True��6D   .��5�   *   �� $
 True��65   $ 	Contained=�6H   5�   '   5�   #   5�      5�      5�      5�      5�      �� 5�   
   "  6   .�5�      ��  � ��$
 10,6q�6(   �� 5�       � ��$	 0,5q�6'   �� 5�       � ��$ 1q�60   �� 5�       � �� �#BDBDBD6,   5�      �� 5�   
   "  6   .�5�      ��  � ��$ 1q�60   �� 5�       � �� �#BDBDBD6,   �� 5�       �@ RowBackground
 �����6.   �� 5�        �@ AlternatingRowBackground �#F5F5F569   �� 5�        �@ HorizontalGridLinesBrush �#E0E0E069   �� 5�       �@ VerticalGridLinesBrush �#E0E0E067   �� 5�       �@ HeadersVisibility$ Column=�62   �� 5�       �@ ColumnHeaderHeight$ 40Q�63   �� 5�       �@ 	RowHeight$ 40Q�6*   �� 5�       �@ AutoGenerateColumns$ False��64   �� 5�       �@ 
IsReadOnly$
 True��6+   �� 5�       �@ SelectionMode$ Single=�6.   �� 5�       �@ SelectionUnit$ FullRow=�6.   5�      �� 5�   
   "  6   .�5�      ��  � �� �#2196F36+   �� 5�       � ��
 �����6+   �� 5�       � ��$ 10q�6(   �� 5�       � ��$ SemiBold"�6+   �� 5�       � ��$
 Left=�6;   5�      5�      .��5�      �  � RowDefinitions 5�   
   �� 5�      $
��Auto �6   �� 5�      $��* �6   .+�5�   
   �� $��0��6   ���#2196F3 �� Padding$ 20,15q�63   ._�5�      �  5�      �� 5�      $��* �6'   �� 5�      $
��Auto �6'   .+�5�      �� $��QUẢN LÝ TÀI KHOẢN��6   
�������$��22(�6H   $
��Bold"�6V   $
��Left=�6h   �� 5�      -   5         ��Name$ BtnOpenAddAccountModal��5�      $��Thêm tài khoản��69   $��1��6R   $
��10,5q�6b   
����������#1E88E5$��1q�6�   ���#1E88E5$y�	Collapsed=�6�   5�      5�      � 5�   
   $��1��6    5�      �� 5�      $	��350 �6#   �� 5�      $��* �6#   .+�5�      �� ���#F5F5F5$ 20q�6*   $��0,0,0,0q�67   ._�5�      �� $
��Auto=�6   .��5�      �� .+�5�      �� $��THÔNG TIN TÀI KHOẢN��6$   $
��Bold"�6?   $��16(�6Q   $��0,0,0,15q�6_   �� 5�      $��Tên đăng nhập:��6$   $��0,5,0,0q�6:   �� 5�      -   5          ��Name$  TxtUsername��5�   "   ! �� 
IsReadOnly$
! True��67   �� 5�      $��
Họ tên:��6$   $��0,10,0,0q�63   �� 5�      -   5        $  TxtFullname��5�   "   $
! True��67   �� 5�      $��Địa chỉ:��6$   $��0,10,0,0q�64   �� 5�      -   5        $  
TxtAddress��5�   "   $
! True��66   �� 5�      $��Số điện thoại:��6$   $��0,10,0,0q�6:   �� 5�      -   5        $  TxtPhone��5�   "   $
! True��64   �� 5�      $��Email:��6$   $��0,10,0,0q�62   �� 5�      -   5        $  TxtEmail��5�   "   $
! True��64   �� 5�      $��Ngày sinh:��6$   $��0,10,0,0q�66   �� 5      -   5        $  DpkDob��5   "   $
! True��62   �� 5     $��Giới tính:��6$   $��0,10,0,0q�66   �� 5     -   5        $  	TxtGender��5  "   $
! True��65   �� 5     $��Trạng thái:��6$   $��0,10,0,0q�67   �� 5     -	   5        " ��Name$" 	CbxStatus��5  #   # �� SelectedValuePath$	# Tag��66   .@�5     �� $��Đang hoạt động��6+   
$ � Tag$$ 1��6D   �� 5     $��Vô hiệu hóa��6+   $$ 0��6A   5	     �� 5     $��Cấp độ tài khoản:��6$   $��0,10,0,0q�6=   �� 5     -
   5        $  TxtAccountRank��5  "   $
! True��6:   � 5      5     �� 5  "   $��* �63   �� 5  "   $��* �63   .+�5     �� -   5        $��	BtnUpdate��5  %   $��0��66   $��30��6F   $��Cập nhật��6R   6e   �� 5     -   5        $��	BtnDelete��5  %   $��1��66   $��30��6F   $
��Xóa��6R   #��[  6`   ${�False��6�   6�   5     �� 5     -   5        $ BtnClear��5  !   $��	Xóa form��63   $��30��6F   ���#757575$��0,10,0,0q�6g   6y   5     5     5     � 5     $��1��6   $��20q�6#    5     �� 5     $
��Auto �6$   �� 5      $��* �6$   .+�5$     � $��0��6   $��0,0,0,15q�6$    5%     �� 5&     $��* �6+   �� 5'     $
��Auto �6+   .+�5*     �� -   5        $  	TxtSearch��5*     $��0��61   $��10q�6A   �� 5+     -   5        $ 	BtnSearch��5+     $��1��60   $��Tìm kiếm��6@   5,       5/     -   5        %  Name$% DgvAccounts��5/     $��1��60   &   AutoGenerateColumns$& False��51     '   Columns' 52     0  *System.Windows.Controls.DataGridTextColumn  53     ,  &System.Windows.Controls.DataGridColumn(   Header$( ID��6-   )   Width5  /System.Windows.Controls.DataGridLengthConverter$	) 120 6W   *   
Visibility$* 	Collapsed=�6c   1  +System.Windows.Controls.DataGridBoundColumn+   Binding+ 69   �� *	AccountId+6c     54     $( Tên đăng nhập��6-   $	) 120 6a   + 6D   �� *
Username+6a     55     $( 	Họ tên��6-   $	) 180 6Z   + 6=   �� *
Fullname+6Z     56     $( Loại tài khoản��6-   $	) 100 6e   + 6E   �� *RoleDisplay+6e     57     $( Email��6-   $	) 180 6V   + 6<   �� *Email+6V     58     $( 
Ngày sinh��6-   $	) 180 6v   + 6@   �� $	 Dob�, �� StringFormat$, 
dd/MM/yyyy��6v     59     $( Số điện thoại��6-   $	) 120 6^   + 6D   �� *Phone+6^     5:     $( Giới tính��6-   $) 80 6b   + 6@   �� *GenderDisplay+6b     5;     $( Trạng thái��6-   $	) 120 6c   + 6A   �� *StatusDisplay+6c     5<     $( Cấp độ��6-   $	) 100 6m   + 6=   �� *AccountRank.AccountRankName+6m   5>     5?     5@     � 5B  
   -   5        - �Name$- AddAccountModal��5B     $y�	Collapsed=�6(   $��2��6?   ���	#80000000.+�5C     �� 
�������$	��600��6(   $	��600��64   $ 8��6D   $ 20q�6U   ._�5D     �  5E     �� 5F     $
��Auto �6(   �� 5G     $��* �6(   �� 5H     $
��Auto �6(   .+�5L     �� $��THÊM TÀI KHOẢN MỚI��6    $
��Bold"�6:   $��18(�6L   $��0��6Z   $��0,0,0,15q�6g   �� 5O     $��1��6#   $
��Auto=�60   .��5P     �� .+�5Q     �� $��Tên đăng nhập *��6(   $��0,5,0,0q�6?   �� 5R     -   5        $  TxtNewUsername��5R  &   �� 5T     $��Mật khẩu *��6(   $��0,10,0,0q�6:   G� 5U     -   5        . G�Name$. TxtNewPassword��5U  *   �� 5W     $��Họ tên *��6(   $��0,10,0,0q�68   �� 5X     -   5        $  TxtNewFullname��5X  &   �� 5Z     $��Địa chỉ *��6(   $��0,10,0,0q�69   �� 5[     -   5        $  TxtNewAddress��5[  &   �� 5]     $��Số điện thoại *��6(   $��0,10,0,0q�6?   �� 5^     -   5        $  TxtNewPhone��5^  &   �� 5`     $��Email *��6(   $��0,10,0,0q�67   �� 5a     -   5        $  TxtNewEmail��5a  &   �� 5c     $��Ngày sinh *��6(   $��0,10,0,0q�6;     5d     -   5        /  Name$/ 	DpkNewDob��5d  )   �� 5f     $��Giới tính *��6(   $��0,10,0,0q�6;   �� 5g     -   5        $" CbxNewGender��5g  '   $	# Tag��6=   .@�5h  "   �� $	��Nam��6/   $$ 0��6=   �� 5i  "   $
��Nữ��6/   $$ 1��6<   5j     �� 5l     $��Trạng thái *��6(   $��0,10,0,0q�6<   �� 5m     -   5        $" CbxNewStatus��5m  '   $	# Tag��6=   .@�5n  "   �� $��Đang hoạt động��6/   $$ 1��6H   �� 5o  "   $��Vô hiệu hóa��6/   $$ 0��6E   5p     �� 5r     $ ��Cấp độ tài khoản *��6(   $��0,10,0,0q�6B   �� 5s     -   5        $" CbxNewAccountRank��5s  '   5t     5u     � 5x     $��2��6   $��0,15,0,0q�6(    5y     �� 5z     $��* �6/   �� 5{     $
��Auto �6/   �� 5|     $
��Auto �6/   .+�5     �� -   5        $ BtnAddAccount��5  !   $��Thêm tài khoản��68   $��1��6Q   $��0,0,10,0q�6a   6s   �� 5�     -   5        $ BtnCancelAddAccount��5�  !   $��Hủy��6>   $��2��6L   ���#7575756q   5�     5�     5�     5�     5�     5�     