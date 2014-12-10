# This is the NOVAMORPHER user login database
# Here you can specify the username, password, and ip
# for a person to log in with. Each username and
# password can be only used with the person with the ip
# Format:
#
# Remember: The username HAS TO be a 1 word, not more
#
# To SPECIFY password type in the following:
#
# $NovaMorpher::UserDB[<Put User Name Here>, password] = <Put User Password Here>;
# Replace <Put User Name Here> with the USERNAME including the "<" and ">"
# Replace the <Put User Password Here> with the PASSWORD including the "<" AND ">"
#
# To SPECIFY IP Address type in the following:
#
# $NovaMorpher::UserDB[<Put User Name Here>, ip] = <Put User IP Here>;
# Replace <Put User Name Here> with the USERNAME including the "<" and ">"
# Replace the <Put User IP Here> with the IP including the "<" AND ">"
#
# Remember: The format of the IP should look like this:
#
# IP:000.000.000.000
# "*" is a wild card, for example, if you want the computer to
# match for 192.168.0.10 but ignore the last number 10 because
# it changes every now and then you would do: IP:192.168.0.*
#
# To SPECIFY STATUS type in the following:
#
# $NovaMorpher::UserDB[<Put User Name Here>, status] = <Put User status Here>;
# Replace <Put User Name Here> with the USERNAME including the "<" and ">"
# Replace the <Put User status Here> with the status including the "<" AND ">"
#
# Remember: The ONLY avalible status currently are:
#
# SuperAdmin
# Admin
# 
#
# FINALLY REMEMBER THE MOST IMPORTANT THING: EVERYTHING HERE IS CASE SENSITIVE!!!!
# WHICH MEANS THAT "test" is not the same thing as "Test" nor is it the same thing
# as "TeSt" so YOUR WARNED!!!!!!

$NovaMorpher::UserDB[VRWarper, password] = "test";
$NovaMorpher::UserDB[VRWarper, ip] = "IP:192.168.*.*";
$NovaMorpher::UserDB[VRWarper, status] = SuperAdmin;

$NovaMorpher::UserDB[Sturm, password] = "Sturm";
$NovaMorpher::UserDB[Sturm, ip] = "IP:65.129.*.*";
$NovaMorpher::UserDB[Sturm, status] = Admin;

$NovaMorpher::UserDB[James, password] = "Jimmy";
$NovaMorpher::UserDB[James, ip] = "IP:24.57.*.*";
$NovaMorpher::UserDB[James, status] = SuperAdmin;

$NovaMorpher::UserDB[Agent, password] = "ATS";
$NovaMorpher::UserDB[Agent, ip] = "IP:172.*.*.*";
$NovaMorpher::UserDB[Agent, status] = SuperAdmin;

$NovaMorpher::UserDB[Vengeance, password] = "Jacob";
$NovaMorpher::UserDB[Vengeance, ip] = "IP:64.165.99.*";
$NovaMorpher::UserDB[Vengeance, status] = SuperAdmin;

$NovaMorpher::UserDB[WarOrphan, password] = "WarOrphan";
$NovaMorpher::UserDB[WarOrphan, ip] = "IP:24.93.239.*";
$NovaMorpher::UserDB[WarOrphan, status] = SuperAdmin;

$NovaMorpher::UserDB[Darky, password] = "Rain";
$NovaMorpher::UserDB[Darky, ip] = "IP:63.34.201.*";
$NovaMorpher::UserDB[Darky, status] = Admin;

$NovaMorpher::UserDB[Grey, password] = "Bot";
$NovaMorpher::UserDB[Grey, ip] = "IP:217.36.40.*";
$NovaMorpher::UserDB[Grey, status] = SuperAdmin;

