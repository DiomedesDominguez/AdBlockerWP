# AdBlockerWP
An AdBlocker for Windows Phone 8 and up.

Hello everyone!

Today I release a beta build of my very first hand made app called AdBlockerWP!

***YOU NEED TO BE INTEROP-UNLOCKED AND HAVE ALL CAPABILITIES UNLOCKED AS WELL!

It is very simple, download the app from the attachments and install it using Windows Phone Power Tools or the use the deployer built into the SDK.


Then run it and then select "Update Hosts File" Then wait.

Boom it has blocked ads for you! Enjoy 

There is also a feature to disable ad blocking.

Please please please let me know how this works for you and what model phone you have as I can only test on emulators and a Lumia 640, but this should work on any Lumia WP8.x Phone.



EDIT:
New Feature(s) Added:
Users can now specify their own URLS to download
Removed 2 default URLS, and changed 1 URL
Current Default Hosts URL List:
https://adaway.org/hosts.txt
https://github.com/StevenBlack/hosts (Using GitHub rawcontent servers to get the HUGE hosts file here which is updated frequently from many sources.

Change Log:
V4: Fixed userList.txt not being found when editing custom user hosts url list.
V5: Added option to edit Windows Hosts file, or just view. (Alpha feature, it is very slow on larger files)
V5.2: Fixed bug on first launch of crashing
V5.3: Default lists can be disabled by user, and better custom list parsing
