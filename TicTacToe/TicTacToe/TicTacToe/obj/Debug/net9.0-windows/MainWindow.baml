   M S B A M L   `   `   `  ����D  @TicTacToe, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:TicTacToe	TicTacToe  5   	   %clr-namespace:TicTacToe	TicTacToe  5   	       TicTacToe.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .
viewmodelsclr-namespace:TicTacToe     5   	   *modelsclr-namespace:TicTacToe     5   	     � Title$  	TicTacToe��5   	   $	��450��6   $	��800��6(   .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��2* �6   .+�5   
   �� $��0��6    �� Orientation$ Vertical=�6%   .+�5      �� $��New Game��6   ��6(   �� *DataContext.SaveGame+ �� RelativeSource ��  �� AncestorType
 Window��6�   �� �� 5      $
��Exit��6   ��6$   �� *DataContext.LoadGame+ �� 
 Window��6�   �� 5      �� 5   
   $��1��66   $	��400��6F   $	��400��6R   
���   �$��1q�6s   ��6   �� *Fields+��5      �� 5      .R�5      A�  A� Columns 6"   �� *Size+ A� Rows 6;   �� *Size+5      ��5      �� 5        � 	Resources 5!       �� 
TargetType	 �� Property
 �� Value��%T   '��       �� 5"      " ��6    .�5#      �� 	 � ��

 �����6;   5$      5%      .R�5&      �� ��6   �� *Value+��67   �� *DataContext.CellClickCommand+ �� 
 Window��6�   �� 5'      5)      5*      5+      