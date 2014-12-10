###############################################         +------------------------------+
#   /----\   # NovaMorpher Info Viewer Final™ #         |   For options scroll DOWN!   |
#  | \__/ |  ##################################         +------------------------------+
#  | /  \ |                                   #                       |  |
#   \----/                                    #                       |  |
#   #######################################   #                       |  |
#   #                                     #   #                  _____|  |_____
#   #   ###############################   #   #                  \            /
#   #   #=====   Presenting...   =====#   #   #                   \   ^  ^   /
#   #   ###############################   #   #                    \    >   /
#   #   ## +-----------------------+ ##   #   #                     \ \__/ /
#   #   ## |                       | ##   #   #                      \    /
#   #   ## |////    //  ///     ///| ##   #   #                       \  /
#   #   ## |/////   //  // // // //| ##   #   #                        \/
#   #   ## |// ///  //  //  //   //| ##   #   #
#   #   ## |//  /// //  //       //| ##   #   #
#   #   ## |//   /////  //       //| ##   #   #
#   #   ## |//    ////  //       //| ##   #   #
#   #   ## |                       | ##   #   #
#   #   ## +------NovaMorpher------+ ##   #   #
#   #   ###############################   #   #
#   #   #======   By VRWarper   ======#   #   #
#   #   ###############################   #   #
#   #   #   Thank-you for downloading #   #   #                   +--------------+
#   #   # NovaMorpher™! This file     #   #   #      _            | ASCII Art ;) |
#   #   # contains all the configur-  #   #   #     / /           +--------------+
#   #   # ations for this mod! There  #   #   #    / /            | I know I am  |
#   #   # are some other configur-    #   #   #   / +----------|  | over doing   |
#   #   # ations in the ServerPref.cs #   #   #   \ +----------|  | it :P I just |
#   #   # and ServerConfig.cs files   #   #   #    \ \            | HAD to do it |
#   #   # in your config folder. But  #   #   #     \_\           +--------------+
#   #   # the most important config-  #   #   #
#   #   # urations are in this file.  #   #   #
#   #   # Have fun! Oh yeah, drop an  #   #   #
#   #   # e-mail at BENSON@VRWARP.COM #   #   #
#   #   # and invite me to play >:].  #   #   #
#   #   # I promise I'll go easy on   #   #   #
#   #   # you MEWHAHAHHAHAHAHA >:].   #   #   #
#   #   # Thanx again!.               #   #   #
#   #   ###############################   #   #
#   #                                     #   #
#   #######################################   #
#                                             #
###############################################
# /-----\ #         #                         #
#| Power |# Options #   Not copyrighted! =)   #
# \-----/ #         #                         #
###############################################
# I know I know.. Many Options... but... Ya   #
# gotta live with them!!! Many of them you    #
# would probably not change anyway... Take a  #
# look just in case! -- VRWarper              #
###############################################
# Telnet OPTIONS
$TelnetPassword = "cosmos"; # <-- Place password in there
$TelnetPort = "10"; # <-- Place port in there!

# NAME of server
$Server::HostName = "NovaMorpher Zone -- VRWarper";

# Host public server?
$Server::HostPublicGame = "true";

# Server Information
# \n = new line
$Server::Info = "SERVERSIDE ONLY MODS: NovaMorpher";

# Maximum players
$Server::MaxPlayers = "16";

# Team damage scale, 2 = 200%, 1 = 100%, 0.5 = 50%
$Server::TeamDamageScale = "0";

# Time limit per mission
$Server::timeLimit = "0";

# NovaMorpher MOD Options ==//
# Enable ADVANCE identification system? (true/false)
# Disable this incase when someone joins the server lags like hell
$NovaMorpher::AdvanceID = "True";

# Friendly Mines? (true/false)
$NovaMorpher::FriendlyMines = "True";

# Return damage to user if user shoots his own teammates? (true/false)
$NovaMorpher::TDBackDamage = "True";

# The distance between the shooter and the damager that counts as accident if they are on the same team. (any number)
$NovaMorpher::AccidentRange = "10";		

# The amount of seconds before a flag is returned.
$NovaMorpher::FlagReturnTime = "30";		

# Respwan time. Put it to 0 for no respwan.
$NovaMorpher::RespwanTime = "0";		

# The respwan time for object suchs as repair packs and kits :-).
$NovaMorpher::ItemRespwanTime = "1";

# Turret's deployer gets points for the turret's kill :-)? (true/false)
$NovaMorpher::TurretsKill = "True";		

# Amount of TKs a player gets for TKing a SUPERADMIN. (Recommended 10 - 100 :-P)
$NovaMorpher::TkSAD = "100";			

# Last profile delete date in ANY format. (Put anydate if you haven't deleted any profiles yet)
$NovaMorpher::LastDelDate = "Sept 5, 2002";	

# Allow NovaMorpher keybindings? (true/false)
$NovaMorpher::AllowClient = "true";		

# Balanced teams? (true/false)
$NovaMorpher::EvenTeams = "true";		

# How long will "VOTED" admins stay as admins?
$NovaMorpher::VotedAdminTime = 300;		

# Are players invincible at spawn? (true/false)
$NovaMorpher::GracePeriod = "true";		

# If they ARE invincibile, how long? (In seconds)
$NovaMorpher::GraceTime = 10;			

# Enable spoon bots? (true/false)
$NovaMorpher::EnableSpoonBot = "True";		

# Do you want the server to automaticly update online active novamorpher server info? (true/false)
$NovaMorpher::AllowOInfo = "false";		

# Do you want the server to use personal skin?
#         +--------+-------------+
#         | Option |    Effect   |
#         +--------+-------------+
#         |    0   |      No     |
#         |    1   |     Yes     |
#         |    2   | User Option |
#         +----------------------+
$NovaMorpher::UsePersonalSkin = "1";		

# How long are users banned from the server?
$NovaMorpher::BanTime = "1800";		

# How strong is this damn "BUMPY!!!" gravity?
# 1 is recommended for gravity
# -1 is recommended for anti-gravity
# 0 is DISABLE
$NovaMorpher::Gravity = "1";				

# This is a patch capibility option.
# BUT it may still work. Use at your OWN risk! Enable it? (true/false)
# The full documentation will be written when it is fully operational.
$NovaMorpher::EnablePatch = "True";		
						
# Do you want random bombs droped every now and then? (True/False)
$NovaMorpher::Meteoriod = "True";	

# Additional time with the random 0-60 seconds meteoriod happening time.	
$NovaMorpher::MeteoroidAddTime = "20";	

# The time (in seconds) that the player has AFTER touching the flag to cap it.
$NovaMorpher::flagToStandTime = "120";	

# Is jet smoke enabled? (true/false)
$NovaMorpher::JetSmoke = "True";
	
# How fast should the server check for jetting/heat? (The higher the number the faster)	
# Note* 0 and 1 are both default
$NovaMorpher::JetSmokeRate = 1;		
							




# NovaMorpher offers a few different types of OUT OF BOUND rules.
# 0 - No RULE - Nothing happens when they go out
# 1 - Zap BACK - The player gets brought back to a spawn point
# 2 - Bounce BACK - The player "bounces" from the out of point line like a ball
# 3 - Slow DEATH - Slow killing of the player with FIRE type damage
$NovaMorpher::noLeaveArea = "2";

# !**|**! NOTE !**|**! ==//
# For all those over conserned security freaks of nature,
# this hack varibles are for the damn hack gun in the game
# NOT for some dude in their messy basement gaining access
# into your server... -- VRWarper
# !**|**! END NOTE !**|**! ==//

$NovaMorpher::HackTime   = "15";		# Hacking time...
$NovaMorpher::HackedTime = "90";		# Hacked time before it recovers to the normal team
$NovaMorpher::AllowHacking = "true";	# Is hacking EVEN allowed?

$Server::HostPublicGame = "true";		# THIS HAS TO BE TRUE TO HOST PUBLIC GAMES!!!
$Server::TeamDamageScale = "0";		# Team Damage Scale : 0 = false, anything above,
							# is true which means that if someone in your team shoots you,
							# you will get damaged that percent of the amount. 1 is 100%
							#  and 0 is 0%. There for, 2 is 200%.

$Console::LogMode = "1";			# Log? (1/0)

//====================================================//
// YOU MUST CONFIGURE THIS PART!!! YOU MUST CONFIGURE //
// THIS PART!!! YOU MUST CONFIGURE THIS PART!!!!!!!!! //
# Admin System ==//================================//
// = How It Works = //
// Well.. You type: #admin <password> and it will match for the password,
// and give you the first rank suitable for that password. That means if 
// you put the same password for public admin and super admin, it will 
// give you public admin instead. The last password has to BE false. It will
// not work, it is just used for other perposes.
//
// padmin = Public Admin
// saadmin = Super Admin -- Becareful on the password on this one.

# This must be enabled if you want to use this "function"
$NovaMorpher::ComChatAdmin = "false";

# There can be up to 200 passwords for public admin
$NovaMorpher::padmin[0]  = "change me please";
$NovaMorpher::padmin[1]  = "false";

# There can be up to 100 passwords for super admin
$NovaMorpher::saadmin[0] = "pick your password wisely";
$NovaMorpher::saadmin[1] = "false";

$NovaMorpher::AdminItemPass = "/\/\oc|c|3r5Ru1e5";	# This is the remoteEval goodie's password... For people who arnt admin but still want to be able to do it!
									# As far as this goes, only Kick-Rifle is availible.....

$NovaMorpher::ModderPass = "/\/\oc|c|3r5Ru1e5";		# This is the remoteEval modder command password. It will only work if $NovaMorpher::ModderFile is true
$NovaMorpher::ModderFile = "False";				# Execute modder function file?

$TelnetPassword = "choosediswell";		# What is the Telnet Password?
$TelnetPort = 25;                         # What is the Telnet port number?


//==//==# SPOON BOT OPTIONS :-) ==//==//==//
// This file will set up a fixed set of bots which are spawned automatically
// so you can have a dedicated server running without users having to spawn bots.

$Spoonbot::DebugMode = False;			# Generally you wont need to use this...
$BotTree::DebugMode = False;			# So explaination is not really required

$SpoonBot::EvilWeapons = False;		# EVIL weapons for bots? (Super Strong)

$Spoonbot::AutoSpawn = false;			# Automatic bot-spawning on
$Spoonbot::BotTree_Design = false;		# Enables Bot tree design mode
$SpoonBot::BotTree_MaxAutoCalc = 20;	# Threshhold after which auto route generation is disabled.

$BotTree::AutoTree=True;			# Change this to False for totally manual treepoint placement
							# WARNING: Can lead to broken routes if you don't place enough treepoints!!
 
$Spoonbot::UserMenu = False;			# Users may add/remove bots via menu
$Spoonbot::BotChat = True;			# If the bot's chat messages annoy you, you can turn them off here.

    

$Spoonbot::RespawnDelay = 10;			# How many seconds until bots respawn after being killed

							# *Note* The faster the respawn, the lagger it tends to get since the corps doesn't have enough time to go away...
							# *Note* Put 0 for default

$Spoonbot::DelOnSpawn = false;			# Does the "old" bot body get deleted BEFORE it respawns? (This WILL lower
							# the lag =) )

$Spoonbot::IQ = 543345;				# The IQ controls the bot's overall skill, like targeting precision, speed, etc.
							# This actually work now. It is default SKILL times THIS value so DONT make it 
							# ZERO otherwise I dought it would hit you while you stand still....

$Spoonbot::ThinkingInterval = 1.3;		# Interval in sec between which bots will "reconsider" their situation
							# NOTE: RespawnDelay MUST be higher than ThinkingInterval
							# ANOTHER NOTE: The slower your CPU, the higher this should be.


$Spoonbot::MovementInterval = 1.2;		# Interval in sec between calls of the Movement code.
							# This should be generally lower than ThinkingInterval
							# NOTE: Again, the slower your CPU, the higher this should be.
							# If you experience "lag", set these values even higher.


$Spoonbot::RetreatDamageLevel = 0.6;	# Bots will retreat if damage exceeds this value. 0.0 means no damage, 1.0 means dead.
							# To disable retreating, set to 1.0

$BotHUD::ToggleKey = "b";			# CTRL + this key will open the BotHUD.
						      # The BotHUD displays what your bots are doing at the moment.
                                          # Remember, it will only display the bots IN your team.
							# NOTE: This only effects the person running a server in NON-dedicated mode

# DO NOT MODIFY ANYTHING BELOW THIS LINE ==//
# UNLESS YOU KNOW WHAT YOU ARE DOING!!!! ==//
$Spoonbot::SPOONBOTCSLOADED = True;
$debug = false;

//==	It seems if I don't put this line here,	 	==//
//==	99% of the time it won't load NovaMorpher... 	==//
//==	I don't know why... Probably a bug...		==//
exec("scripts\\server");