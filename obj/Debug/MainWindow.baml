   M S B A M L   `   `   `  ����F  BBetterMusic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:BetterMusicBetterMusic  5   	       BetterMusic.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:BetterMusic     5   	   -   5          �Name$  Better_Music��5   	    � Title$ Better Music��5   	    � Icon$ disc.ico��6   $	��450��6.   $	��800��6;    � 	Resources 5	         play 	 pause  next  back  mute  
volumehigh  	volumelow 
 folder  	playwhite 	 
pausewhite 
 	nextwhite  	backwhite  	mutewhite  volumehighwhite  volumelowwhite  musicnotewhite  folderwhite��%o	  &	        &	      &	 �     &	 ~    &	     &	 �    &	     &	 �    &	 
    &		 �    &	
     &	 �    &	     &	 �    &	 -    &	 �    &	 ?    �� 5
   
   $O��IC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/play.png��6   $��24��6p   $��24��6|   �� 5   
   $P��JC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/pause.png��6   $��24��6r   $��24��6~   �� 5   
   $O��IC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/next.png��6   $��24��6p   $��24��6|   �� 5   
   $S��MC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/previous.png��6   $��24��6t   $��24��6�   �� 5   
   $O��IC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/mute.png��6   $��24��6p   $��24��6|   �� 5   
   $U��OC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/volumehigh.png��6#   $��24��6|   $��24��6�   �� 5   
   $T��NC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/volumelow.png��6"   $��24��6z   $��24��6�   �� 5   
   $Q��KC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/folder.png��6   $��24��6t   $��24��6�   �� 5   
   $T��NC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/playwhite.png��6"   $��24��6z   $��24��6�   �� 5   
   $U��OC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/pausewhite.png��6#   $��24��6|   $��24��6�   �� 5   
   $T��NC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/nextwhite.png��6"   $��24��6z   $��24��6�   �� 5   
   $X��RC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/previouswhite.png��6"   $��24��6~   $��24��6�   �� 5   
   $T��NC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/mutewhite.png��6"   $��24��6z   $��24��6�   �� 5   
   $Z��TC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/volumehighwhite.png��6(   $��24��6�   $��24��6�   �� 5   
   $Y��SC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/volumelowwhite.png��6'   $��24��6�   $��24��6�   �� 5   
   $Y��SC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/musicnotewhite.png��6'   $��24��6�   $��24��6�   �� 5   
   $V��PC:\Users\Matt\source\repos\BetterMusic/BetterMusic/assetts/icons/folderwhite.png��6$   $��24��6~   $��24��6�   5      .��5      � -   5         �Name$ bgGrid��5      .+�5   
   �� -   5         ��Name$ playBtn��5      $
��Left=�6"   $��348,381,0,0q�6=   $	��Top=�6R   $��75��6j    J� RenderTransformOrigin$ 0.5,1&�6�   
������ 
������ .��5      �� .+�5       �� $��assetts/icons/play.png��6   $��24��68   $��24��6D   5!      5"      �� 5#   
   -   5        $ skipBay��5#      $
��Left=�6"   $��447,381,0,0q�6=   $	��Top=�6R   $��75��6j   $ 0.5,1&�6�   
������ 
������ .��5$      �� .+�5%      �� $��assetts/icons/next.png��6   $��24��68   $��24��6D   5&      5'      �� 5(   
   -   5        $ BackBtn��5(      $
��Left=�6"   $��255,381,0,0q�6=   $	��Top=�6R   $��75��6j   $ 0.5,1&�6�   
������ 
������ .��5)      �� .+�5*      �� $ ��assetts/icons/previous.png��6   $��24��6<   $��24��6H   5+      5,      �� 5-   
   -   5         ��Name$ musicImg��5-      $
��Left=�6"   $	��230��6=   $��
255,50,0,0q�6J   $	��Top=�6^   $	��267��6v   $��assetts/cassette.jpg��6�   $
��Fill=�6�   �� 5.   
   -   5         ��Name$ timeCounter��5.      $�� ��6%   $��Right=�60   $
��Left=�6S   $��255,336,0,0q�6n   $	��Top=�6�   $	��267��6�   $
��Bold"�6�   $��18(�6�   �� 5/   
   -   5        $ 	songTitle��5/      $�� ��6#   $��Center=�6.   $��255,280,270,0q�6R   $	��Top=�6i   $
��Bold"�6�   $��24(�6�   �� 50   
   -	   5        $ 
fileDialog��50      $
��Left=�6%   $��
32,381,0,0q�6@   $	��Top=�6T   $	��100��6l   $	 0,1&�6�   
������ 
������ .��51      �� .+�52      ��  
 Browse#��[ 6   $��24��69   $��24��6E   53      54      �� 55   
   -
   5        	 ��Name$	 songMenu��55      $
��Left=�6$   $	��360��6?   $��	10,10,0,0q�6L   $��0q�6_   $	��Top=�6s   $	��143��6�   $ 0,0.5&�6�   $��Hidden=�6  $��Hidden=�6C  �� 56   
   -   5        $
 play��56      $��Select��6   $
��Left=�60   $��-168,343,0,0q�6K   $	��Top=�6a   $��75��6y   $	 0,1&�6�   �� 57   
   -   5        
 ��Name$
 	volSlider��57      $
��Left=�6$   $��656,389,0,0q�6?   $	��Top=�6T   $	��126��6l    � Value$	 100Q�6x   $	 1,1&�6�    � Maximum$	 100Q�6�   �� 58   
   -   5        $ volSlide��58      $��Volume��6"   $
��Left=�63   $��656,363,0,0q�6N   $	��Top=�6c   $	 1,1&�6{   �� 59   
   -   5        $
 progressSlider��59      $
��Left=�6)   $��255,363,0,0q�6D   $	��Top=�6Y   $ -1.752,0.021&�6q   $	��267��6�   6�   �� 5:   
   -   5        $
 mute��5:      $
��Left=�6   $��624,381,0,0q�6:   $	��Top=�6O   $��32��6g   
������ 
������ .��5;      �� -   5         ��Name$ volPanel��5;      .+�5<      �� -   5        $ muteImg��5<        
VolumeHigh#��[ 6)   $��24��6N   $��24��6Z   6e   5=      5>      �� 5?   
   -   5        $ 
ArtistName��5?      $�� ��6$   $��Center=�6/   $
��Left=�6S   $��255,310,0,0q�6n   $	��Top=�6�   $	��267��6�   �� 5@   
   -   5        $ MusicTab��5@      $
��Left=�6#   $��-365,342,0,0q�6>   $	��Top=�6T   $��72��6l   $	 0,1&�6�   
������ 
������ .��5A      �� .+�5B      ��   	MusicNote#��[ 6   $��24��6<   $��24��6H   5C      5D      �� 5E   
   -   5        $ plusImg��5E      $	��158��6!   $��311,83,330,0q�6.   $	��Top=�6D   $��assetts/icons/plus.png��6\   $y�Hidden=�6|   �� 5F   
   -   5        $ AddPlaylist��5F      $��Button��6&   $
��Left=�67   $��-168,404,0,-5q�6R   $	��Top=�6i   $��75��6�   5H      5I      