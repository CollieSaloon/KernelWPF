   M S B A M L   `   `   `  ����K  GKernelTestingWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:KernelTestingWPFKernelTestingWPF  5      #    KernelTestingWPF.FastSlowPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:KernelTestingWPF     5        N� Title$  FastSlow Page��5	      .��5      � 
������� � RowDefinitions 5   
   �� 5      $	��.5* �6   �� 5      $��3* �6   �� 5      .+�5   
   � .+�5      �� -   5        $��txtTitle��5      $��Center=�6(   $��Center=�6E   $��Running Program: ��6`   $��Calibri)�6y   $��24(�6�   5      � 5   
   $��1��6    5      �� 5      �� 5      $��2* �6    �� 5      $��5* �6    �� 5       � ColumnDefinitions 5      �� 5      $
��.75* �6#   �� 5      $��5* �6#   �� 5      $
��.15* �6#   �� 5      $��2* �6#   �� 5       $	��.3* �6#   .+�5"      � $��1��6   $��1��6     5#      �� 5$      �� 5%      $��0.15* �6$    5'      �� 5(      $	��.3* �6'   �� 5)      $��6* �6'   �� 5*      $	��.3* �6'   .+�5,      �� -   5        $��myScrollView��5,      $��1��63   $��Stretch=�6C   $��Stretch=�6a   $
��Auto=�5-      $
��Auto=�6>   .��5.      �� -   5        $��ScrollStackPanel��5.   !   $��Stretch=�69   $��Center=�6U    �� Orientation$ 
Horizontal=�6r   50      51      53      � 54      $��2��6   $��1��6     55      �� 56      �� 57      $��7* �6$    59      �� 5:      �� 5;      $��5* �6'   �� 5<      �� 5=      $��5* �6'   �� 5>      .+�5@      �� -   5        $��slowListView��5@      $��3��6/   $��1��6?   $��Stretch=�6L   $��Stretch=�6j   $��Center=�6�   �� 5A      -   5        $��fastListView��5A      $��1��6/   $��1��6?   $��Stretch=�6L   $��Stretch=�6j   $��Center=�6�   �� 5B      $��
Fast Queue��6   $��Center=�6.   $��Center=�6K   $��1��6f   �� 5C      $��
Slow Queue��6   $��Center=�6.   $��Center=�6K   $��3��6f   5D      �� 5F      -   5        $��listViewInstructions��5F      $��3��63   $��2��6C   $��Center=�6P   �� 5G      $y�	Collapsed=�6   $ 
Horizontal=�60   $��1��6I   .+�5H      �� -   5        $��Button 1��5H      $��Center=�6,   $��Center=�6G   $��0,0,5,0q�5J      
�������
���   �6=   �� 5K      -   5        $��Button 2��5K      $��Center=�6,   $��Center=�6G   $��0,0,5,0q�5M      
�������
���   �6=   �� 5N      $��Button 3��6   $��Center=�6,   $��Center=�6G   $��0,0,5,0q�5O      
�������
���   �6=   5P      �� 5R      $��2��6   $��1��6&   $ 
Horizontal=�66   $��Center=�6O   $��Center=�6j   5S      � 5T   
   $��2��6    5U      �� 5V      $��3* �6#   �� 5W      .+�5Y      �� $
��Auto=�6   .��5Z      �� -	   5        $��txtInfo��5Z      $��InfoText��6+   $��5,0,0,0q�6;   5[      �� 5\      -
   5        $��GoToReportButton��5\      
�������
���   �$��80��5]   <   $��2��5^      $��2��6!   $��View Report��6.   $��Center=�5_      $��Center=�6.   5`      5b      5c      