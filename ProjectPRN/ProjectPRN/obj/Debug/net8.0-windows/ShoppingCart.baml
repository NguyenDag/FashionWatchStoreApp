   M S B A M L   `   `   `  ����  
ProjectPRN    ProjectPRN.ShoppingCart   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Giỏ hàng��5   	   $	��550��6   $	��800��6'   ���#F5F5F5 � WindowState$ 	Maximized=�5   	   .��5      � .+�5   
   �� $��60��6   $	��Top=�6   ���#007ACC K� ZIndex$ 1��6J   ._�5	      � $��Center=�6    � ColumnDefinitions 5
      �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   .+�5      �� -   5        $��0��5      $��🏠 Trang chủ��6)   
�������
������ $��0q�5   =   $��16(�5      $
��Bold"�6   $
��20,0q�61   6?   �� 5      -   5         ��Name$ btnShop��5      $��1��6*   $��🛒 Sản phẩm��6:   
�������
������ $��0q�5   =   $��16(�5      $
��Bold"�6   $
��20,0q�61   6?   �� 5      -   5        $ btnCart��5      $��2��6*   $��📞 Giỏ hàng��6:   
�������
������ $��0q�5   =   $��16(�5      $
��Bold"�6   $
��20,0q�61   6?   �� 5!      -   5        $ btnLogin��5!      $��3��6+   $��🔐 Đăng nhập��6;   
�������
������ $��0q�5"   =   $��16(�5#      $
��Bold"�6   $
��20,0q�61   6?   �� 5&      -   5        $ btnRegister��5&      $��4��6.   $��📝 Đăng ký��6>   
�������
������ $��0q�5'   =   $��16(�5(      $
��Bold"�6   $
��20,0q�61   6?   �� 5+      -   5        $ btnViewInfo��5+      $��3��6.   $��👤 Thông tin��6>   
�������
������ $��0q�5,   =   $��16(�5-      $
��Bold"�6   $
��20,0q�61   $y�	Collapsed=�5.      �� 52      -   5        $ 	btnLogout��52      $��4��6,   $��🚪 Đăng xuất��6<   
�������
������ $��0q�53   =   $��16(�54      $
��Bold"�6   $
��20,0q�61   $y�	Collapsed=�55      57      58      � 5:   
   $��10,70,10,10q�6    5;      �� 5<      $��2* �6#   �� 5=      $
��1.5* �6#   .+�5A      �� 
������� �� Padding$ 10q�6(    �� CornerRadius$ 10��65   ���#B0BEC5$��1q�6]   ._�5B      �� .+�5C      �� $!��🛒 Danh sách giỏ hàng��6    $��20(�6=   $
��Bold"�6K   ���#37474F$��0,0,0,10q�6r   �� 5D      $	��350��6A   
���������6   �� *	CartItems+ �� View 5E      �� 5F      .P�5H   "   �� $��Chọn��61    �� Width$ 50��6?   	 �� CellTemplate	 5I   &   �� 5J   *   .R�5K   .   �� 
 d� 	IsChecked
 67   �� *
IsSelected+ �� Mode$ TwoWay=�5L   +   5N   #   �� 5P   "   $��Hình ảnh��61   $ 80��6C   	 5Q   &   �� 5R   *   .R�5S   .   �� $��50��6Y   $��50��6d   ��64   �� *Product.MainImage+6d   5T   +   5V   #   �� 5Y   "   $��Tên sản phẩm��61   $	 180��6t    �� DisplayMemberBinding 6G   �� *ProductName+6t   �� 5\   "   $
��Giá��61   $ 80��6i    6>   �� *	PriceSell+6i   �� 5_   "   $��Số lượng��61   	 5`   &   �� 5a   *   .R�5b   .   ��  �� Orientation$ 
Horizontal=�69   $��Center=�6R   .+�5d   2   �� -	   5        $��-��5d   9   $��25��6E   $��25��6P   
���  ��
�������
 � Tag 5e   8   �� *	ProductId+�� 5h   2   $��50��5i      ���#ECEFF1$��Center=�6:    �� 
IsReadOnly$
 True��5j      ��5h   :   �� *
Quantity+5j      �� 5m   2   -
   5        $��+��5m   9   $��25��6E   $��25��6P   
��� � �
������� 5n   8   �� *	ProductId+5o   /   5p   +   5r   #   �� 5v   "   $��Tổng tiền��61   $	 100��6p    6D   �� *
TotalPrice+6p   �� 5y   "   $
��Xóa��61   $ 70��6>   	 5z   &   �� 5{   *   .R�5|   .   �� -   5        $	��❌��5|   5   $��40��6A   $��40��6L   
���  ��
������� 5}   Q   �� *	ProductId+5~   +   5�   #   5�      5�      5�      5�      �� 5�      $��1��6   
�������$ 10q�68   $ 10��6E   $��10,0,0,0q�6W   ���#B0BEC5$��1q�6   ._�5�      �� .+�5�      �� $"��✅ Sản phẩm đã chọn��6    $��20(�6:   $
��Bold"�6H   ���#37474F$��0,0,0,10q�6o   �� 5�      $	��250��6E   
���������6   �� *SelectedItems+ 5�      �� 5�      .P�5�   "   �� $��Tên sản phẩm��61   $	 150��6t    6G   �� *ProductName+6t   �� 5�   "   $��Tổng tiền��61   $	 100��6p    6D   �� *
TotalPrice+6p   5�      5�      �� 5�      $ 
Horizontal=�6!   $��0,10,0,0q�6:   $��Center=�6L   .+�5�      �� $��Tổng thanh toán: ��6$   $��18(�6=   $
��Bold"�6K   ���#455A646]   �� 5�      $��18(�6C   $
��Bold"�6Q   ���#D32F2F��6$   �� *CheckoutTotal+6c   5�      �� 5�      -   5        $��🛍️ Thanh toán��5�      $	��150��66   $��40��6B   ���#43A047
�������$
��Bold"�6v   $��0q�6�    � Cursor$
 Hand��6�   $��0,10,0,0q�6�   6�   5�      5�      5�      5�      5�      