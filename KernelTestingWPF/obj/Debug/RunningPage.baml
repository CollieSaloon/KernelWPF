   M S B A M L   `   `   `  ����K  GKernelTestingWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:KernelTestingWPFKernelTestingWPF  5      "    KernelTestingWPF.RunningPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:KernelTestingWPF     5        N� Title$  Running Page��5	      .��5      � 
������� � RowDefinitions 5   
   �� 5      $	��.5* �6   �� 5      $��3* �6   �� 5      .+�5   
   � .+�5      �� -   5        $��txtTitle��5      $��Center=�6(   $��Center=�6E   $��Running Program: ��6`   $��Calibri)�6y   $��24(�6�   5      � 5   
   $��1��6    5      �� 5      �� 5      $��5* �6    �� 5       � ColumnDefinitions 5      �� 5      $
��.75* �6#   �� 5      $��5* �6#   �� 5      $
��.15* �6#   �� 5      $��2* �6#   �� 5      $	��.3* �6#   .+�5!      �� -   5        $��listViewInstructions��5!      $��3��63   $��1��6C   $��Center=�6P   �� 5"       �� Orientation$ 
Horizontal=�6   $��1��62   .+�5#      �� -   5        $��  -  ��5#      $��Center=�6)   $��Center=�6D   $��0,0,5,0q�5%      
�������
���   �6=   �� 5&      -   5        $��  +  ��5&      $��Center=�6)   $��Center=�6D   $��0,0,5,0q�5(      
�������
���   �6=   �� 5)      $��Button 3��6   $��Center=�6,   $��Center=�6G   $��0,0,5,0q�5*      
�������
���   �6=   5+      �� 5,      -   5        $��myScrollView��5,      $��1��6/   $��1��6<   $��Stretch=�6L   $��Stretch=�6j   $
��Auto=�5-      $
��Auto=�6>   .��5.      �� -   5        $��ScrollStackPanel��5.      $��Stretch=�65   $ 
Horizontal=�6Q   50      51      �� 52      $��2��6   $��1��6&   $ 
Horizontal=�66   $��Center=�6O   $��Center=�6j   53      � 54   
   $��2��6    55      �� 56      $��3* �6#   �� 57      .+�59      �� $
��Auto=�6   .��5:      �� -   5        $��txtInfo��5:      $��InfoText��6+   $��5,0,0,0q�6;   5;      �� 5<      -   5        $��GoToReportButton��5<      
�������
���   �$��80��5=   <   $��2��5>      $��2��6!   $��View Report��6.   $��Center=�5?      $��Center=�6.   5@      5B      5C      