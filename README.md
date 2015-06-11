====================================================================
Copyright
====================================================================
Checksum Verifier Utility
Copyright 2013 Chase (carnageX) <carnagex@outlook.com>
This work is free. You can redistribute it and/or modify it under the
terms of the Do What The Fuck You Want To Public License, Version 2,
as published by Sam Hocevar. See http://www.wtfpl.net/ for more details.

====================================================================
Warranty
====================================================================
/* This program is free software. It comes without any warranty, to
     * the extent permitted by applicable law. You can redistribute it
     * and/or modify it under the terms of the Do What The Fuck You Want
     * To Public License, Version 2, as published by Sam Hocevar. See
     * http://www.wtfpl.net/ for more details. */

====================================================================
Details & Usage
====================================================================
Built on .NET Framework 4.5

Supported Checksum Algorithms: 
	MD5, MD5-CNG
	SHA-1, SHA-1 Managed, SHA-1 CNG
	SHA-256, SHA-256 Managed, SHA-256 CNG
	SHA-384, SHA-384 Managed, SHA-384 CNG
	SHA-512, SHA-512 Managed, SHA-512 CNG
	RIPEMD160, RIPEMD160 Managed
	CRC16
	CRC32

Supports 3 modes: 
	Single File
	Multiple File
	Text string
	
You can generate hashes for either mode, or compare to a checksum provided to the program by the user. 

To use (single or Multiple File modes):
	1) Select desired algorithm(s) (default is MD5 - assumed this would be most used)
	2) Select a tab (mode) to use
	3) (Optional) If verifying a file's checksum, enter it in the "Checksum" field. 
	4) Browse for a file to generate
	5) Click the Compare button 
		Note: for large files it may take some time... be patient!
	6) The result will appear in the appropriate section in the tab's section.
		a) For Single File mode: the checksum generated for the file will appear in the "File Checksum" field.  If comparing to an entered Checksum, the result will display below the File Checksum field, noting if it is a match or mismatch.  The checksum generated from the file will also turn green for a match, and red for a mismatch. 
		
		b) For Multiple Files mode: The generated checksum(s) will appear in the list to the right of the Browse and Compare buttons for each file.  It will also note if there was a match or mismatch to the given Checksum (if one was given). 
		
To use Text String Mode: 
  1) Select desired algorithm(s)
  2) Input a text string in the "Input String" box
    a) Input a checksum to compare against if desired
  3) Select an encoding method
  4) Click Generate/Compare button
  
====================================================================		
License	Content
====================================================================
DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE 
                    Version 2, December 2004 

 Copyright (C) 2004 Sam Hocevar <sam@hocevar.net> 

 Everyone is permitted to copy and distribute verbatim or modified 
 copies of this license document, and changing it is allowed as long 
 as the name is changed. 

            DO WHAT THE FUCK YOU WANT TO PUBLIC LICENSE 
   TERMS AND CONDITIONS FOR COPYING, DISTRIBUTION AND MODIFICATION 

  0. You just DO WHAT THE FUCK YOU WANT TO.
  
====================================================================		
Changelog
====================================================================

Version 4.3.0
  Added Statusbar added to bottom of window
    Various details such as current operation status (Ready, Running, Finished, and Error), elapsed operation time, and the progress bar from each tab has been removed and added to statusbar
  
Version 4.2.1
  Added Comparison capability for Text tab
  Switched order of File / Checksum inputs on Single File tab
  Added thousands-separator for file size
  
Version 4.2.0
  Added Text (Single string) Hash Generation tab with the following text encoding options: 
    ASCII, BigEndianUnicode, Default (current system's default), Unicode, UTF7, UTF8, UTF32

Version 4.1.0
  Added additional hash algorithms: 
    MD5-CNG, SHA1 Managed/CNG, SHA256 Managed/CNG, SHA384 Managed/CNG, SHA512 Managed/CNG, and RIPEMD160 Managed
    
Version 4.0.0
  Initial release of WPF conversion from WinForms
