   M S B A M L   `   `   `  ����  
ProjectPRN    ProjectPRN.ChangePassword   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Đổi Mật Khẩu��5   	   $	��300��6   $	��400��6+    � WindowStartupLocation$ CenterScreen=�67   .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� $��🔑 Đổi Mật Khẩu��6   $��16(�6+   $
��Bold"�69   $��Center=�5      $��0,0,0,15q�61   �� 5   
   $��1��6   $	��0,5q�6"   .+�5      �� $ ��Mật khẩu hiện tại:��6   G� 5      -   5         G�Name$ pbOldPassword��5      5      �� 5   
   $��2��6   $	��0,5q�6"   .+�5      �� $��Mật khẩu mới:��6   G� 5      -   5        $ pbNewPassword��5      5      �� 5    
   $��3��6   $	��0,5q�6"   .+�5!      �� $ ��Nhập lại mật khẩu:��6   G� 5"      -   5        $ pbConfirmPassword��5"      5#      �� 5&   
   $��4��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��0,15,0,0q�6W   .+�5'      �� -   5        $
��Lưu��5'      $��80��6#   6.   �� 5(      -   5        $��Hủy��5(      $��80��6#   $��10,0,0,0q�6.   6@   5)      5*      5+      