   M S B A M L   `   `   `  ����J  FMinesweeper.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Minesweeper.AppMinesweeper.App  5   	   1clr-namespace:Minesweeper.AppMinesweeper.App  5	   	        Minesweeper.App.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:Minesweeper.App     5   	   4
viewmodelsclr-namespace:Minesweeper.App     5	   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6   .+�5   
   �� $
��10pxq�6   .+�5      �� Minesweeper6%   �� 5       �� Orientation$ 
Horizontal=�6   .+�5      �� Size:6#   �� 5      $��100px��6   ��6(   �� *Size+5      �� 5      $ 
Horizontal=�6   .+�5      �� Mines:6$   �� 5       � Minimum$ 1Q�6    � Maximum$ 16Q�6%   $��100px��6O    �� TickFrequency$ 1Q�6]    �� IsSnapToTickEnabled$
 True��6o    � Value 62   �� *
MinesCount+6o   �� 5      ��6   �� *
MinesCount+5      �� 5!      ��6   �� *StartGameCommand+.��6:   
Start Game6F   5#      �� 5%   
   $��1��6   ��6'   �� *Fields+��5&      �� 5'      .R�5(      A�  A� Columns 6"   �� *Size+5)      ��5+      �� 5,      	 � 	Resources	 5-      
 �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value��%d  '��       �� 5.      "
 ��6    .�5/      ��  � ��
 �����6;   �� 51       � ��$ 24(�69    52      �� 53   "    6.   �� *Value+ -16H   .a�55   &   ��  � ��
 �2�2�6C   56   #   �� 57   "    6.   �� *Value+ 06H   .a�58   &   ��  � ��
 �����6C   59   #   �� 5:   "    6.   �� *Value+ 16H   .a�5;   &   ��  � ��
 ��d�6C   5<   #   �� 5=   "    6.   �� *Value+ 26H   .a�5>   &   ��  � ��
 � � �6C   5?   #   �� 5@   "    6.   �� *Value+ 36H   .a�5A   &   ��  � ��
 �  ��6C   5B   #   �� 5C   "    6.   �� *Value+ 46H   .a�5D   &   ��  � ��
 �� ��6C   5E   #   �� 5F   "    6.   �� *Value+ 56H   .a�5G   &   ��  � ��
 � ���6C   5H   #   �� 5I   "    6.   �� *Value+ 66H   .a�5J   &   ��  � ��
 ��ح�6C   5K   #   �� 5L   "    6.   �� *Value+ 76H   .a�5M   &   ��  � ��
 �   �6C   5N   #   �� 5O   "    6.   �� *Value+ 86H   .a�5P   &   ��  � ��
 �����6C   5Q   #   5S      5T      .R�5U      �� ��6   �� *Value+��67   �� *DataContext.ClickFieldCommand+ �� RelativeSource ��  �� AncestorType
 Window��6�   �� 5V      5X      5Y      5Z      