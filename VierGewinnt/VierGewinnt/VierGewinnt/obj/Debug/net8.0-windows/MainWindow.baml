   M S B A M L   `   `   `  ����F  BVierGewinnt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:VierGewinntVierGewinnt  5   	       VierGewinnt.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:VierGewinnt     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��4* �6   .+�5   
   �� $��0��6    �� Orientation$ Vertical=�6%   .+�5      �� -   5        $��Neues Spiel��5      6+   �� 5      $��Columns:��6   �� 5      -   5        $��ColumnSlider��5       � Maximum$ 8Q�6)    � Minimum$ 4Q�65    �� TickFrequency$ 1Q�6Z    �� IsSnapToTickEnabled$
 True��6l    � Value 6A   �� *Column+6l   �� 5      -   5        $��ColumnLabel��5      ��6'   ��  �� ElementName$ ColumnSlider��	 �� Path$	 Value��� 5      $��Rows:��6   �� 5      -   5        $��	RowSlider��5      $ 8Q�6&   $ 4Q�62   $ 1Q�6T   $
 True��6f    6>   �� *Row+6f   �� 5      -   5        $��RowLabel��5      ��6$   �� $ 	RowSlider��$	 Value�5      �� 5   
   $��1��6   ��6"   �� *Field+��5      �� 5      .R�5      A� 
 A� Columns
 6"   �� *Column+ A� Rows 6=   �� *Row+5      ��5"      �� 5#       � 	Resources 5$       �� 
TargetType �� Property �� Value �� Triggers ^� 
Conditions �� Binding �� Value P� Property P� Value��%�  '��       �� 5%      " ��6    .�5&      ��  � ��
 �����6;   �� 5'       � ��$ 40(�69    5)      ^� 5+   "    5,   &   �� 5-   *    64   �� *Status+ 16O   .0�5/   &   ��  � ��
 ��  �6C   �� 50   &    � ��$ X��6@   51   #   ^� 54   "    55   &   �� 56   *    64   �� *Status+ 26O   .0�58   &   ��  � ��
 �  ��6C   �� 59   &    � ��$ O��6@   5:   #   ^� 5=   "    5>   &   �� 5?   *    64   �� *Status+ 06O   .0�5A   &   ��  � ��
 �����6C   �� 5B   &    � ��$  ��6@   5C   #   P� 5F   "    �@J�IsMouseOver$
 True��6A   . �5G   &   ��  � ��
 �   �6D   �� 5H   &    � ��$ 2q�6H   5I   #   5W      5X      .R�5Y      �� $��75��6   $��75��6(   $�� ��64   $��Stretch=�6?   $��Stretch=�6]   ��6y   �� *DataContext.MakeMoveCommand+ �� RelativeSource ��  �� AncestorType
 Window��6�   �� *X+5Z      5\      5]      5^      