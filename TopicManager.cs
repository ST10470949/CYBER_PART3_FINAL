using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurity_Part2._2
{
    public static class TopicManager
    {
        public static readonly Dictionary<string, string> Topics = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
        {
            { "password",
              "🔐 STRONG PASSWORD SECURITY\n\n" +

              "📖 What it is:\n" +
              "A password is your first line of defence against unauthorised access. Strong password security means creating passwords that are extremely difficult for attackers or automated tools to guess or crack.\n\n" +

              "💡 Analogy:\n" +
              "Think of your password like a house key. A weak password is a key thousands of houses share. A strong unique password fits only your lock.\n\n" +

              "✅ What a strong password looks like:\n" +
              "• At least 12-16 characters long\n" +
              "• Mix of uppercase, lowercase, numbers and symbols\n" +
              "• Weak: password123 | Strong: T!g3r#Sky92@Blue\n" +
              "• Best: a passphrase like 'Coffee&Rain!Makes_Me_Happy2024'\n\n" +

              "⚠ Signs your password may be compromised:\n" +
              "• Login alerts from locations you don't recognise\n" +
              "• Your account sends messages you didn't write\n" +
              "• Unfamiliar purchases on your accounts\n\n" +

              "🌍 Real World Example:\n" +
              "In 2012, LinkedIn had 117 million passwords stolen. Users who reused those passwords on banking or email had those accounts compromised too — even though those sites were never hacked.\n\n" +

              "🛠 Recommended Tools:\n" +
              "• Bitwarden — free, open-source\n" +
              "• LastPass — popular and easy to use\n" +
              "• 1Password — great for families\n" +
              "• KeePass — offline, privacy-focused\n\n" +

              "🚨 If your password is stolen:\n" +
              "1. Change it immediately on that account\n" +
              "2. Change it on every site where you reused it\n" +
              "3. Enable 2FA on the affected account\n" +
              "4. Check haveibeenpwned.com for your email\n" +
              "5. Monitor bank statements for suspicious activity"
            },

            { "phishing",
              "🎣 PHISHING ATTACKS\n\n" +

              "📖 What it is:\n" +
              "Phishing is a cyberattack where criminals impersonate trusted organisations to trick you into revealing passwords, banking details or personal data.\n\n" +

              "💡 Analogy:\n" +
              "Imagine a letter that looks exactly like it came from your bank — same logo, same tone — but was written by a criminal. That is phishing.\n\n" +

              "🎯 Types of Phishing:\n" +
              "• Email Phishing — fake emails from banks, Netflix, PayPal or government\n" +
              "• Spear Phishing — targeted attacks using your personal details\n" +
              "• Smishing — phishing via SMS (e.g. 'Your package is held, click here')\n" +
              "• Vishing — phishing over a phone call pretending to be your bank\n" +
              "• Clone Phishing — a real email you received is copied with a malicious link\n\n" +

              "⚠ Warning Signs:\n" +
              "• Urgent language like 'Your account closes in 24 hours'\n" +
              "• Sender address doesn't match the real company domain\n" +
              "• Poor grammar and spelling in the message\n" +
              "• Links show a different URL when you hover over them\n\n" +

              "🌍 Real World Example:\n" +
              "In 2016, Hillary Clinton's campaign chairman clicked a fake Google password-reset email. His entire inbox was stolen and published. One click caused one of the most damaging political leaks in history.\n\n" +

              "🚨 If you clicked a phishing link:\n" +
              "1. Disconnect from the internet immediately\n" +
              "2. Run a full antivirus scan\n" +
              "3. Change passwords for any accounts you entered\n" +
              "4. Enable 2FA on those accounts\n" +
              "5. Check bank statements for unauthorised transactions\n" +
              "6. Report to cybercrimehub@saps.gov.za if in South Africa"
            },

            { "malware",
              "🛡 MALWARE — MALICIOUS SOFTWARE\n\n" +

              "📖 What it is:\n" +
              "Malware is software deliberately designed to damage, disrupt or gain unauthorised access to a device. It is one of the most common tools cybercriminals use.\n\n" +

              "💡 Analogy:\n" +
              "Think of malware like a biological virus for your computer. It sneaks in, replicates and causes damage — often without you knowing it is there.\n\n" +

              "🎯 Types of Malware:\n" +
              "• Virus — attaches to files and spreads when shared\n" +
              "• Worm — spreads across networks automatically\n" +
              "• Trojan — disguises itself as legitimate software\n" +
              "• Spyware — silently monitors your activity and keystrokes\n" +
              "• Ransomware — encrypts your files and demands payment\n" +
              "• Keylogger — records every key you press to steal passwords\n\n" +

              "⚠ Signs your device is infected:\n" +
              "• Running unusually slowly for no clear reason\n" +
              "• Pop-ups appearing constantly even without a browser open\n" +
              "• Programs opening or closing on their own\n" +
              "• Browser homepage changed without your permission\n" +
              "• Antivirus has been disabled and you can't turn it back on\n\n" +

              "🌍 Real World Example:\n" +
              "In 2017, WannaCry ransomware infected 200,000 computers in 150 countries in one day. The UK's NHS had surgeries cancelled and ambulances diverted. Damage: $4 billion — all because organisations hadn't installed a critical Windows update.\n\n" +

              "🛠 Free Removal Tools:\n" +
              "• Malwarebytes Free — excellent scanner\n" +
              "• Windows Defender — built into Windows 10/11\n" +
              "• Avast Free Antivirus — well-rounded protection\n\n" +

              "🚨 If your device is infected:\n" +
              "1. Disconnect from the internet immediately\n" +
              "2. Run a full scan using Malwarebytes\n" +
              "3. Boot into Safe Mode and scan again\n" +
              "4. Change all passwords from a clean device\n" +
              "5. Restore files from a clean backup if needed"
            },

            { "social engineering",
              "🎭 SOCIAL ENGINEERING\n\n" +

              "📖 What it is:\n" +
              "Social engineering targets human psychology rather than technical systems. Attackers manipulate people into giving away confidential information or performing actions that compromise security.\n\n" +

              "💡 Analogy:\n" +
              "Imagine a stranger in a delivery uniform saying confidently 'I need access to the server room.' Most people let them in without question — that is social engineering. Authority and confidence bypass security.\n\n" +

              "🎯 Types of Social Engineering:\n" +
              "• Pretexting — fake scenario to extract info (e.g. fake IT technician needing your login)\n" +
              "• Baiting — infected USB drives left in public hoping someone plugs them in\n" +
              "• Tailgating — physically following someone through a secure door\n" +
              "• Quid Pro Quo — offering something in exchange for your password\n" +
              "• Vishing — voice phishing pretending to be your bank or government\n\n" +

              "⚠ Warning Signs:\n" +
              "• Unusual urgency or panic to make you act fast\n" +
              "• Being asked to bypass normal security procedures 'just this once'\n" +
              "• Someone claiming high authority you cannot verify\n" +
              "• Being asked for info the real company should already have\n\n" +

              "🌍 Real World Example:\n" +
              "In 2020, Twitter attackers called employees pretending to be IT staff. They got credentials to internal admin tools and hijacked Obama, Elon Musk and Apple's accounts. Over $100,000 stolen in hours — all from one phone call.\n\n" +

              "🚨 If you were socially engineered:\n" +
              "1. Report to your IT security team or manager immediately\n" +
              "2. Change any passwords or credentials you shared\n" +
              "3. Document everything you remember about the attacker\n" +
              "4. Alert colleagues so they can watch for the same attack\n" +
              "5. Never give passwords to anyone claiming to be IT support"
            },

            { "2fa",
              "🔒 TWO-FACTOR AUTHENTICATION (2FA)\n\n" +

              "📖 What it is:\n" +
              "2FA adds a second layer of security beyond your password. Even if an attacker steals your password, they cannot access your account without the second factor.\n\n" +

              "💡 Analogy:\n" +
              "Think of an ATM — you need the physical card AND your PIN. Stealing just one is not enough. 2FA works exactly the same way for online accounts.\n\n" +

              "🎯 Types of 2FA:\n" +
              "• SMS Code — one-time code via text (convenient but least secure)\n" +
              "• Authenticator App — time-based code every 30 seconds (much more secure)\n" +
              "• Hardware Key — physical USB device like a YubiKey\n" +
              "• Biometric — fingerprint or facial recognition\n\n" +

              "🛠 Recommended Authenticator Apps:\n" +
              "• Google Authenticator — simple and widely supported\n" +
              "• Microsoft Authenticator — great backup support\n" +
              "• Authy — best option, supports multi-device backup\n\n" +

              "🏆 Accounts to prioritise for 2FA first:\n" +
              "1. Your primary email — everything else resets through it\n" +
              "2. Online banking and financial accounts\n" +
              "3. Social media accounts\n" +
              "4. Work accounts and cloud storage\n\n" +

              "🌍 Real World Example:\n" +
              "In 2019, SIM-swapping attacks drained millions in cryptocurrency from victims using SMS-based 2FA. Users with authenticator apps were completely unaffected — the physical SIM swap gave attackers nothing.\n\n" +

              "🚨 If your 2FA is compromised:\n" +
              "1. Log in and disable the compromised 2FA method immediately\n" +
              "2. Set up a new authenticator app on a fresh device\n" +
              "3. Change your password on that account\n" +
              "4. Review recent account activity for unauthorised access"
            },

            { "safe browsing",
              "🌐 SAFE BROWSING\n\n" +

              "📖 What it is:\n" +
              "Safe browsing means taking deliberate precautions to protect yourself online. Most cyberattacks begin with a single careless click — good habits dramatically reduce your risk.\n\n" +

              "💡 Analogy:\n" +
              "Browsing without safety habits is like walking through an unfamiliar city at night with your wallet hanging out. Most streets are fine — but without awareness you won't know the dangerous ones until it's too late.\n\n" +

              "🔍 HTTP vs HTTPS:\n" +
              "• HTTP — data is unencrypted, anyone on the network can read it\n" +
              "• HTTPS — data is encrypted, cannot be intercepted\n" +
              "• Always look for the padlock in your browser before entering personal info\n" +
              "• Never enter passwords or payment details on HTTP-only sites\n\n" +

              "🔍 How to check if a link is safe:\n" +
              "• Hover over links — check the actual URL at the bottom of your browser\n" +
              "• Use virustotal.com to scan suspicious URLs\n" +
              "• Use unshorten.it to reveal where shortened links actually go\n\n" +

              "🛠 Recommended Browser Extensions:\n" +
              "• uBlock Origin — blocks ads, trackers and malicious scripts\n" +
              "• Privacy Badger — automatically blocks invisible trackers\n" +
              "• Bitwarden — autofills passwords only on the correct website\n\n" +

              "🌍 Real World Example:\n" +
              "In 2021, CryptoRom targeted users through dating apps with fake crypto investment sites. Users deposited real money — when they tried to withdraw, sites vanished. Over $1.4 billion stolen globally through unverified links.\n\n" +

              "🚨 If you visited a dangerous website:\n" +
              "1. Close the browser tab immediately\n" +
              "2. Clear your browser cache and cookies\n" +
              "3. Run a full antivirus scan\n" +
              "4. Change any passwords you entered on that site\n" +
              "5. Check bank statements if financial info was entered"
            },

            { "ransomware",
              "💣 RANSOMWARE\n\n" +

              "📖 What it is:\n" +
              "Ransomware encrypts all files on your device making them completely inaccessible. The attacker then demands payment — usually in cryptocurrency — for the decryption key.\n\n" +

              "💡 Analogy:\n" +
              "Imagine coming home to every room padlocked from outside. A note says: 'Pay R50,000 in Bitcoin and we'll give you the keys. 72 hours.' Your files are still there — but you cannot reach any of them.\n\n" +

              "🎯 How Ransomware Spreads:\n" +
              "• Phishing emails with infected attachments (most common)\n" +
              "• Clicking malicious links in emails or websites\n" +
              "• Downloading cracked software or pirated content\n" +
              "• Unpatched security vulnerabilities in outdated software\n\n" +

              "⚠ Signs you may have ransomware:\n" +
              "• Files have strange new extensions like .locked or .encrypted\n" +
              "• You cannot open documents or photos that worked before\n" +
              "• A ransom note appears on your screen\n" +
              "• Device runs extremely slowly as files are being encrypted\n\n" +

              "🌍 Real World Examples:\n" +
              "• WannaCry (2017) — 200,000 computers, 150 countries, $4 billion damage\n" +
              "• Colonial Pipeline (2021) — US fuel pipeline offline 6 days, $4.4 million ransom paid\n" +
              "• Garmin (2020) — GPS company offline for days, estimated $10 million ransom\n\n" +

              "❓ Should you pay the ransom:\n" +
              "No. Payment doesn't guarantee file recovery, funds criminals, and marks you as a willing target for future attacks.\n\n" +

              "🚨 If ransomware hits:\n" +
              "1. Disconnect from Wi-Fi and unplug all network cables immediately\n" +
              "2. Do NOT pay the ransom\n" +
              "3. Check nomoreransom.org for free decryption tools\n" +
              "4. Restore from a clean offline backup\n" +
              "5. Wipe and reinstall the OS if no decryption tool exists"
            },

            { "public wifi",
              "📶 PUBLIC WI-FI SAFETY\n\n" +

              "📖 What it is:\n" +
              "Public Wi-Fi in cafes, airports and hotels is frequently unsecured and heavily targeted by cybercriminals.\n\n" +

              "💡 Analogy:\n" +
              "Using public Wi-Fi without protection is like having a private conversation in a crowded restaurant. Anyone nearby can hear every word. A VPN is a soundproof bubble — only you and the other person can hear.\n\n" +

              "🎯 How Attackers Exploit Public Wi-Fi:\n" +
              "• Man-in-the-Middle Attack — attacker intercepts everything you send and receive\n" +
              "• Evil Twin Attack — fake hotspot named 'Starbucks_Free_WiFi' captures all your traffic\n" +
              "• Packet Sniffing — software captures and reads unencrypted data on the network\n" +
              "• Session Hijacking — stealing your session cookies to take over logged-in accounts\n\n" +

              "⚠ How to Spot a Fake Hotspot:\n" +
              "• Ask staff for the exact official Wi-Fi name — don't guess\n" +
              "• Be suspicious of networks requiring no password\n" +
              "• Watch for duplicate names with slight differences\n\n" +

              "🛠 Recommended Free VPN Options:\n" +
              "• ProtonVPN Free — no data limit, based in Switzerland\n" +
              "• Windscribe Free — 10GB per month free\n" +
              "• TunnelBear Free — easy to use, 500MB per month\n\n" +

              "🌍 Real World Example:\n" +
              "In 2017 Kaspersky researchers set up a fake hotspot at a major tech conference. Within hours, dozens of security professionals had connected. Login credentials, emails and financial data were intercepted — in under 30 minutes.\n\n" +

              "🚨 If you connected to a suspicious network:\n" +
              "1. Disconnect immediately and forget the network\n" +
              "2. Change passwords for any accounts you accessed\n" +
              "3. Enable 2FA on those accounts\n" +
              "4. Run a malware scan\n" +
              "5. Check bank and email for unauthorised activity"
            },

            { "identity theft",
              "🕵 IDENTITY THEFT\n\n" +

              "📖 What it is:\n" +
              "Identity theft occurs when someone steals your personal information and uses it to impersonate you — opening bank accounts, applying for credit or committing crimes in your name.\n\n" +

              "💡 Analogy:\n" +
              "Someone uses a copy of your ID to take a R200,000 loan in your name and disappears. You find out three months later when debt collectors call. Recovering from it can take years.\n\n" +

              "🎯 How Identity Theft Happens:\n" +
              "• Phishing attacks stealing your ID numbers and banking details\n" +
              "• Data breaches from companies storing your information\n" +
              "• Social media oversharing — posting your ID, address or birthdate publicly\n" +
              "• Physical theft of your wallet, ID or personal mail\n" +
              "• Fake job applications asking for your ID and banking details upfront\n\n" +

              "⚠ Signs your identity may be stolen:\n" +
              "• Bills or statements for accounts you never opened\n" +
              "• Credit applications rejected despite a good credit history\n" +
              "• Debt collectors contacting you about debts you don't recognise\n" +
              "• Your SASSA or SARS account shows activity you didn't do\n\n" +

              "🔍 How to check:\n" +
              "• Check credit report at TransUnion or Experian (free annually in South Africa)\n" +
              "• Visit haveibeenpwned.com for your email in known breaches\n" +
              "• Contact SAFPS to check for impersonation flags on your identity\n\n" +

              "🌍 Real World Example:\n" +
              "The 2017 Equifax breach exposed 147 million people's names, ID numbers and financial records. Victims spent over 200 hours each over several years clearing fraudulent credit applications filed in their names.\n\n" +

              "🚨 Steps to recover:\n" +
              "1. Report to SAPS and get a case number\n" +
              "2. Contact SAFPS at 0860 101 248 for protective registration\n" +
              "3. Notify your bank and request new account numbers\n" +
              "4. Place a credit freeze with all three credit bureaus\n" +
              "5. Report to SARS if your tax profile may be compromised"
            },

            { "updates",
              "⬆ SOFTWARE UPDATES\n\n" +

              "📖 What it is:\n" +
              "Software updates patch bugs, improve performance and — most critically — close security vulnerabilities that attackers can exploit. Keeping software updated is one of the simplest and most effective cybersecurity actions you can take.\n\n" +

              "💡 Analogy:\n" +
              "Think of your software like a house. Over time, locks wear out and gaps appear that burglars exploit. Updates are builders fixing every crack and reinforcing every lock. Postpone them and the gaps keep growing.\n\n" +

              "🎯 What is a Zero-Day Vulnerability:\n" +
              "A zero-day is a flaw known to attackers but not yet fixed by the developer. Once a patch is released, install it immediately — from that moment attackers know exactly what the vulnerability was and will exploit unpatched systems.\n\n" +

              "⚠ Real Consequences of Ignoring Updates:\n" +
              "• WannaCry attacked only computers missing a Windows update available two months earlier\n" +
              "• The Equifax breach happened because a known patch wasn't applied for two months\n" +
              "• Outdated browsers are the most common entry point for drive-by download attacks\n\n" +

              "🛠 How to Enable Automatic Updates:\n" +
              "• Windows: Settings → Windows Update → Check for Updates\n" +
              "• Android: Settings → Software Update → Download and Install\n" +
              "• iPhone: Settings → General → Software Update\n" +
              "• Mac: Apple Menu → System Preferences → Software Update\n\n" +

              "🌍 Real World Example:\n" +
              "Equifax ignored a critical patch for 78 days. Attackers exploited it during that window and stole 147 million people's financial records. The CEO resigned. $700 million in settlements. All preventable by one update.\n\n" +

              "🚨 Best Practices:\n" +
              "1. Enable automatic updates on your OS, browser and antivirus\n" +
              "2. Update phone apps weekly\n" +
              "3. Replace unsupported software like Windows 7\n" +
              "4. Update your router firmware — most people never do this"
            },

            { "encryption",
              "🔑 ENCRYPTION\n\n" +

              "📖 What it is:\n" +
              "Encryption converts readable data into unreadable scrambled format using a mathematical algorithm. Only someone with the correct decryption key can convert it back. It protects data whether stored on a device or travelling across the internet.\n\n" +

              "💡 Analogy:\n" +
              "Writing a letter and locking it in a box with a unique key before posting it. Even if someone intercepts the box, they cannot read the letter without the key.\n\n" +

              "🎯 Types of Encryption:\n" +
              "• Symmetric — one key for encrypting and decrypting. Fast. Example: AES.\n" +
              "• Asymmetric — public key encrypts, private key decrypts. Example: RSA.\n" +
              "• End-to-End (E2EE) — encrypted on your device, decrypted only on the recipient's. Used by WhatsApp, Signal, iMessage.\n" +
              "• TLS — encrypts data between your browser and websites. This is what HTTPS uses.\n\n" +

              "🌐 Messaging App Encryption:\n" +
              "• Signal — gold standard for private messaging, fully open source\n" +
              "• WhatsApp — uses Signal's E2EE protocol for messages\n" +
              "• Telegram — only encrypts in 'Secret Chat' mode. Regular chats are NOT end-to-end encrypted.\n\n" +

              "🌍 Real World Example:\n" +
              "In 2016 the FBI demanded Apple create a backdoor to decrypt a terrorist's iPhone. Apple refused. The FBI paid over $1 million to a third party to crack it. Strong encryption — properly implemented — resisted even government-level attempts.\n\n" +

              "🚨 Encryption Best Practices:\n" +
              "1. Enable full-disk encryption — BitLocker on Windows, FileVault on Mac\n" +
              "2. Use Signal or WhatsApp for sensitive conversations\n" +
              "3. Always use HTTPS websites — check for the padlock\n" +
              "4. Encrypt sensitive files before cloud storage using Veracrypt\n" +
              "5. Use a strong password to protect encrypted files or drives"
            },

            { "firewalls",
              "🧱 FIREWALLS\n\n" +

              "📖 What it is:\n" +
              "A firewall monitors and filters all incoming and outgoing network traffic based on predefined rules. It acts as a gatekeeper between your trusted network and untrusted external networks like the internet.\n\n" +

              "💡 Analogy:\n" +
              "A firewall is like a security guard at a building entrance. Every person entering or leaving is checked against a list. Approved visitors get through. Unauthorised ones are turned away. The guard keeps a log of everyone.\n\n" +

              "🎯 Types of Firewalls:\n" +
              "• Windows Built-in Firewall — adequate for home users, free with Windows 10/11\n" +
              "• Third-Party Software Firewalls — ZoneAlarm, Comodo — more control and logging\n" +
              "• Hardware Firewalls — physical router-based devices protecting all network devices\n" +
              "• Next-Generation Firewalls (NGFW) — enterprise-grade with deep packet inspection\n\n" +

              "🔍 How to Check Your Windows Firewall:\n" +
              "1. Search 'Windows Security' in the Start Menu\n" +
              "2. Click 'Firewall and Network Protection'\n" +
              "3. Ensure it shows 'On' for Domain, Private and Public networks\n" +
              "4. If it shows 'Off' — turn it on immediately\n\n" +

              "⚠ What a Firewall Cannot Protect You From:\n" +
              "• Malware arriving through allowed traffic like email attachments\n" +
              "• Insider threats from someone already inside the network\n" +
              "• Social engineering attacks — firewalls can't stop humans being manipulated\n\n" +

              "🌍 Real World Example:\n" +
              "In 2003 the SQL Slammer worm infected 75,000 computers and slowed the internet worldwide in just 10 minutes. Organisations with firewalls blocking the specific UDP port were completely unaffected. Those without firewalls were infected within seconds.\n\n" +

              "🚨 Firewall Best Practices:\n" +
              "1. Always keep Windows Firewall turned on — never disable it\n" +
              "2. Review firewall rules regularly and remove unnecessary ones\n" +
              "3. Enable logging to see what traffic is being blocked\n" +
              "4. Never create exceptions for applications you don't recognise\n" +
              "5. Use a firewall alongside antivirus — they protect against different threats"
            },

            { "backup & recovery",
              "💾 BACKUP AND RECOVERY\n\n" +

              "📖 What it is:\n" +
              "Backup and recovery means creating secure copies of your data so that if the original is lost, corrupted or stolen, you can restore it. It is your ultimate safety net against ransomware, hardware failure and human error.\n\n" +

              "💡 Analogy:\n" +
              "Backups are like house insurance. You hope you never need it — but if disaster strikes, you are incredibly relieved it exists. Without it, everything you worked for is simply gone.\n\n" +

              "🎯 The 3-2-1 Backup Rule:\n" +
              "• 3 — Keep three copies of your data\n" +
              "• 2 — Store them on two different types of media\n" +
              "• 1 — Keep one copy completely offsite or offline\n\n" +

              "🛠 Recommended Backup Tools:\n" +
              "Cloud: Google Drive (15GB free), OneDrive (5GB free), Backblaze ($9/month unlimited)\n" +
              "Local: External hard drive, USB drive, NAS for multi-device homes\n\n" +

              "⏰ How Often to Back Up:\n" +
              "• Daily — for work files and documents you edit regularly\n" +
              "• Weekly — for photos, videos and personal projects\n" +
              "• Monthly — for full system images\n\n" +

              "🔍 Testing Your Backup:\n" +
              "Restore a random file from your backup monthly to verify it works. A backup never tested is not a backup — it is hope.\n\n" +

              "🌍 Real World Example:\n" +
              "In 2021, ransomware hit Ireland's Health Service and shut down their entire healthcare IT system. Their backup systems were also connected to the same network and got encrypted too. Recovery cost over €100 million. Offline backups would have changed everything.\n\n" +

              "🚨 If you lose your data:\n" +
              "1. Stop using the affected device immediately\n" +
              "2. Try Recuva (free) for accidentally deleted files\n" +
              "3. Check nomoreransom.org if ransomware is involved\n" +
              "4. Restore from your most recent clean backup\n" +
              "5. Contact a professional recovery service for physical drive damage"
            },

            { "incident response",
              "🚨 INCIDENT RESPONSE\n\n" +

              "📖 What it is:\n" +
              "Incident response is the structured process of detecting, containing, eliminating and recovering from a cybersecurity incident — a hack, data breach, malware infection or ransomware attack.\n\n" +

              "💡 Analogy:\n" +
              "Think of incident response like a fire drill. No one expects a fire, but every building has an evacuation plan and fire extinguishers. When the alarm goes off, everyone knows exactly what to do without panicking.\n\n" +

              "🎯 The 6 Phases of Incident Response:\n" +
              "1. Preparation — having a plan and tools ready before anything happens\n" +
              "2. Identification — detecting the incident and understanding its scope\n" +
              "3. Containment — isolating affected systems to stop the attack spreading\n" +
              "4. Eradication — removing the threat completely\n" +
              "5. Recovery — restoring systems and data to normal safely\n" +
              "6. Lessons Learned — reviewing what happened and how to prevent it next time\n\n" +

              "👤 What to Do if You Get Hacked:\n" +
              "1. Disconnect from the internet and all networks immediately\n" +
              "2. Change passwords for all important accounts from a clean device\n" +
              "3. Enable 2FA on all accounts\n" +
              "4. Run a full antivirus and malware scan\n" +
              "5. Notify your bank if financial accounts may be affected\n\n" +

              "📞 Who to Contact in South Africa:\n" +
              "• Your bank — call the number on the back of your card\n" +
              "• SAPS Cybercrime Hub — cybercrimehub@saps.gov.za\n" +
              "• CERT-SA — South Africa's Computer Emergency Response Team\n\n" +

              "🌍 Real World Example:\n" +
              "In 2013, Target detected suspicious network activity but didn't act for several days. By then, 40 million credit card numbers and 70 million personal records had been stolen. Immediate response could have contained it within hours. Cost: over $300 million.\n\n" +

              "🚨 Incident Response Checklist:\n" +
              "1. Disconnect affected devices immediately\n" +
              "2. Document everything — time, symptoms, actions taken\n" +
              "3. Change all compromised credentials\n" +
              "4. Notify bank, employer and police if needed\n" +
              "5. Restore from clean backups after scanning"
            },

            { "privacy settings",
              "🔐 PRIVACY SETTINGS\n\n" +

              "📖 What it is:\n" +
              "Privacy settings let you control how your personal data is collected, stored, shared and used. Most defaults are configured to share as much data as possible — changing them is one of the most impactful things you can do.\n\n" +

              "💡 Analogy:\n" +
              "Imagine a new house where all windows and curtains are permanently open by default. Anyone walking by sees everything inside. Privacy settings let you choose which curtains to close and who gets a key.\n\n" +

              "🎯 Platform-Specific Settings to Change Right Now:\n\n" +
              "Facebook:\n" +
              "• Who can see your posts → set to 'Friends' not 'Public'\n" +
              "• Search engine link to profile → turn OFF\n" +
              "• Ad Preferences → turn off personalised advertising\n\n" +
              "Google Account (myaccount.google.com):\n" +
              "• Turn off Web and App Activity\n" +
              "• Turn off Location History\n\n" +
              "iPhone:\n" +
              "• Settings → Privacy → Location Services → set each app to 'While Using' or 'Never'\n" +
              "• Settings → Privacy → Tracking → turn OFF\n\n" +
              "Browser:\n" +
              "• Use Firefox or Brave — both have strong privacy by default\n" +
              "• Use DuckDuckGo as your search engine\n\n" +

              "⚠ Why This Matters:\n" +
              "Companies collect your location, search history, purchases and app usage to build a profile for targeted advertising. When a company is breached, all that collected data ends up in criminal hands.\n\n" +

              "🌍 Real World Example:\n" +
              "In 2018, Cambridge Analytica harvested 87 million Facebook users' data without consent through a personality quiz app. That data built psychological profiles used for political targeting. All affected users were on Facebook's permissive default settings.\n\n" +

              "🚨 Privacy Checklist:\n" +
              "1. Review app permissions on your phone monthly\n" +
              "2. Set all social media profiles to private\n" +
              "3. Disable location tracking for apps that don't need it\n" +
              "4. Opt out of personalised advertising on all platforms\n" +
              "5. Read privacy policies before installing free apps"
            },

            { "iot security",
              "📡 IOT SECURITY — INTERNET OF THINGS\n\n" +

              "📖 What it is:\n" +
              "IoT security covers protecting internet-connected devices beyond computers — smart TVs, security cameras, smart speakers, baby monitors, thermostats and smart locks. These devices often have very weak built-in security.\n\n" +

              "💡 Analogy:\n" +
              "Every IoT device added to your home network is another door to your house. A smart TV with default credentials is a door with no lock. An attacker only needs one unlocked door to access your entire network.\n\n" +

              "🎯 Real IoT Devices That Have Been Hacked:\n" +
              "• Baby Monitors — thousands of live feeds accessible publicly due to unchanged default passwords\n" +
              "• Smart TVs — Samsung TVs found to have backdoor microphone access\n" +
              "• Security Cameras — 2021 Verkada breach gave hackers access to 150,000 cameras in hospitals, prisons and Tesla factories\n" +
              "• Home Routers — Mirai botnet infected 600,000 routers and launched the largest DDoS attack in history\n\n" +

              "🛡 Router Security Settings to Change Now:\n" +
              "1. Change the default admin username and password immediately\n" +
              "2. Update router firmware via the admin panel\n" +
              "3. Disable Remote Management unless specifically needed\n" +
              "4. Enable WPA3 encryption on your Wi-Fi\n" +
              "5. Create a separate Guest Wi-Fi network for all IoT devices\n" +
              "6. Disable UPnP — it allows devices to open ports that attackers exploit\n\n" +

              "🌍 Real World Example:\n" +
              "In 2016 the Mirai botnet found 600,000 IoT devices using factory default passwords, infected them and used them to take down Twitter, Netflix, Reddit and Spotify across the USA and Europe for a full day. The weapon: passwords never changed from the factory default.\n\n" +

              "🚨 IoT Security Checklist:\n" +
              "1. Change default passwords on every IoT device immediately\n" +
              "2. Put all IoT devices on a separate Guest Wi-Fi\n" +
              "3. Update firmware on every device regularly\n" +
              "4. Disable features you don't use — remote access, UPnP\n" +
              "5. Scan your network with the Fing app to spot unknown devices"
            },

            { "mobile security",
              "📱 MOBILE SECURITY\n\n" +

              "📖 What it is:\n" +
              "Mobile security involves protecting your smartphone from malware, data theft, unauthorised access and physical theft. Your phone contains your emails, banking apps, photos and messages — making it one of the most valuable targets for cybercriminals.\n\n" +

              "💡 Analogy:\n" +
              "Your smartphone knows where you are at all times, has access to your bank account and contains years of personal conversations. Leaving it unprotected is like carrying your entire life in an unlocked bag in a crowded shopping centre.\n\n" +

              "🎯 Android vs iPhone Security:\n" +
              "Android:\n" +
              "• More flexible but higher risk from third-party apps\n" +
              "• More targeted by malware due to larger market share\n" +
              "• Install apps only from the Google Play Store\n\n" +
              "iPhone:\n" +
              "• More locked-down, stronger default security\n" +
              "• All apps reviewed by Apple before App Store listing\n" +
              "• iMessage and FaceTime end-to-end encrypted by default\n\n" +

              "⚠ Signs your phone may be compromised:\n" +
              "• Battery draining much faster than usual\n" +
              "• Phone heating up when not in use\n" +
              "• Strange texts being sent from your number that you didn't send\n" +
              "• New apps appearing that you didn't install\n\n" +

              "🔍 If your phone is lost or stolen:\n" +
              "• Android: android.com/find — locate, lock or remotely erase\n" +
              "• iPhone: icloud.com/find — locate, lock, enable Lost Mode or erase\n\n" +

              "🌍 Real World Example:\n" +
              "In 2021 the Pegasus spyware infected phones of journalists, activists and world leaders including Emmanuel Macron. It activated cameras, read encrypted messages and tracked location without any knowledge. Good everyday habits prevent the vast majority of less sophisticated attacks.\n\n" +

              "🚨 Mobile Security Checklist:\n" +
              "1. Set a strong PIN or biometric lock — never use a simple pattern\n" +
              "2. Only install apps from official app stores\n" +
              "3. Review app permissions after every new install\n" +
              "4. Enable Find My Device and remote wipe\n" +
              "5. Keep your OS and apps updated\n" +
              "6. Back up your phone weekly"
            },

            { "secure coding",
              "💻 SECURE CODING\n\n" +

              "📖 What it is:\n" +
              "Secure coding means writing software with security built in from the start — not added as an afterthought. It means anticipating how an attacker might misuse your code and writing it in a way that prevents those attacks.\n\n" +

              "💡 Analogy:\n" +
              "You wouldn't build a house with no locks and open access to the electrical panel then add security after a break-in. You design security in from the beginning. Secure coding does the same for software.\n\n" +

              "🎯 OWASP Top 10 — Most Critical Web Security Risks:\n" +
              "1. Broken Access Control — users accessing data they shouldn't\n" +
              "2. Cryptographic Failures — storing sensitive data without encryption\n" +
              "3. Injection — SQL injection allowing malicious input to manipulate systems\n" +
              "4. Insecure Design — fundamental flaws no patching can fully fix\n" +
              "5. Security Misconfiguration — default settings, open cloud storage\n" +
              "6. Vulnerable Components — libraries with known security flaws\n" +
              "7. Authentication Failures — weak login and session management\n" +
              "8. Data Integrity Failures — trusting data without verifying it\n" +
              "9. Security Logging Failures — not detecting or recording attacks\n" +
              "10. Server-Side Request Forgery — tricking the server into unintended requests\n\n" +

              "🛠 Security Testing Tools:\n" +
              "• SonarQube — static code scanning, integrates with CI/CD pipelines\n" +
              "• OWASP ZAP — free dynamic testing for web applications\n" +
              "• Burp Suite — industry standard for web security testing\n\n" +

              "🌍 Real World Example:\n" +
              "The 2017 Equifax breach was caused by an unpatched SQL injection vulnerability in Apache Struts. A patch had been available for two months. Attackers queried the database directly and extracted 147 million people's financial records. Basic input validation would have prevented it entirely.\n\n" +

              "🚨 Secure Coding Checklist:\n" +
              "1. Always validate and sanitise user input\n" +
              "2. Use parameterised queries to prevent SQL injection\n" +
              "3. Apply least privilege — give code only the access it needs\n" +
              "4. Keep all libraries and dependencies updated\n" +
              "5. Store passwords using bcrypt or Argon2 — never plain text\n" +
              "6. Reference owasp.org for up-to-date guidance"
            },

            { "network segmentation",
              "🕸 NETWORK SEGMENTATION\n\n" +

              "📖 What it is:\n" +
              "Network segmentation divides a network into smaller isolated sections. Each segment only communicates with others when absolutely necessary, limiting how far an attacker can move if they breach one segment.\n\n" +

              "💡 Analogy:\n" +
              "A hospital designed as one giant open room — patients, staff, medicine storage and records all sharing the same space. One contagious person infects everyone. Instead, hospitals have separate locked wards. Network segmentation does this for your digital infrastructure.\n\n" +

              "🎯 Technologies Used:\n" +
              "• VLANs — logical separation of traffic on the same physical hardware\n" +
              "• Firewalls — control traffic between segments based on rules\n" +
              "• Subnets — dividing an IP network into smaller address ranges\n" +
              "• Zero Trust Architecture — verifies every user and device regardless of location\n\n" +

              "🏠 Home Network Segmentation — Simple and Practical:\n" +
              "• Create a separate Guest Wi-Fi for visitors\n" +
              "• Connect all IoT devices to the Guest network — not your main network\n" +
              "• Most modern routers support multiple SSIDs — use this feature\n\n" +

              "🏢 Small Business Implementation:\n" +
              "• Separate customer Wi-Fi from the internal office network\n" +
              "• Put point-of-sale systems on their own isolated VLAN\n" +
              "• Keep accounting and HR on a more restricted segment\n\n" +

              "🌍 Real World Example:\n" +
              "The 2013 Target breach started through a compromised HVAC contractor with network access. Because Target's network wasn't segmented, attackers moved from the contractor's access all the way to payment systems. 40 million credit card numbers stolen. Proper segmentation would have contained the breach to one isolated segment.\n\n" +

              "🚨 Network Segmentation Checklist:\n" +
              "1. Set up a separate Guest Wi-Fi for visitors and IoT devices\n" +
              "2. Never connect untrusted devices to your main network\n" +
              "3. Use your router's VLAN features if available\n" +
              "4. Block traffic between segments by default — allow only what is needed\n" +
              "5. Scan your network regularly for unknown devices"
            },

            { "access control",
              "🔒 ACCESS CONTROL\n\n" +

              "📖 What it is:\n" +
              "Access control ensures people, systems and applications can only access the resources they are specifically authorised to use — nothing more. It is the digital equivalent of deciding who gets a key and who is not allowed in at all.\n\n" +

              "💡 Analogy:\n" +
              "In an office building, a cleaner has a key to every office but not the server room. A junior accountant can view reports but not approve payments. The CEO accesses everything. Each person has exactly the access they need for their job — and nothing more.\n\n" +

              "🎯 Authentication vs Authorisation:\n" +
              "• Authentication — proving who you are (password, fingerprint, 2FA)\n" +
              "• Authorisation — determining what you are allowed to do once identified\n" +
              "• Authentication is the security guard checking your ID. Authorisation is the list of rooms you're permitted to enter.\n\n" +

              "🎯 Access Control Models:\n" +
              "• RBAC (Role-Based) — permissions assigned by job role\n" +
              "• ABAC (Attribute-Based) — access based on department, location, time of day\n" +
              "• MAC (Mandatory) — system itself enforces access levels, used in high-security environments\n" +
              "• DAC (Discretionary) — resource owner decides who has access\n\n" +

              "⚠ What Happens When Access Control Fails:\n" +
              "• Employee with excessive permissions accidentally deletes critical database records\n" +
              "• Former contractor whose access was never revoked steals data months later\n" +
              "• Compromised admin account gives attacker complete system control\n\n" +

              "🌍 Real World Example:\n" +
              "The 2020 Twitter hack used social engineering to get credentials from employees who had far more access than their jobs required. Attackers hijacked Obama, Elon Musk and Apple's accounts. Proper least-privilege control would have limited what those credentials could touch.\n\n" +

              "🚨 Access Control Best Practices:\n" +
              "1. Apply least privilege — give only the minimum access needed\n" +
              "2. Remove access immediately when an employee leaves\n" +
              "3. Use MFA for all privileged accounts\n" +
              "4. Conduct access reviews every quarter\n" +
              "5. Use separate admin accounts — never browse or email with admin credentials"
            },

            { "security policies",
              "📜 SECURITY POLICIES\n\n" +

              "📖 What it is:\n" +
              "Security policies are documented rules that define how an organisation or individual manages cybersecurity. They set the expectations, responsibilities and procedures everyone must follow to keep systems and data safe.\n\n" +

              "💡 Analogy:\n" +
              "Security policies are like the rules of the road. They exist not to inconvenience people but to keep everyone safe. Without them, every person makes their own decisions and chaos follows.\n\n" +

              "🎯 What a Basic Security Policy Should Cover:\n" +
              "• Password Policy — minimum length, complexity, change frequency\n" +
              "• Acceptable Use Policy — what devices may be used for, personal use rules\n" +
              "• Data Protection Policy — how sensitive data is classified, stored and shared\n" +
              "• Incident Response Policy — what to do and who to call when something goes wrong\n" +
              "• Software Update Policy — how quickly patches must be applied\n" +
              "• BYOD Policy — rules for using personal devices for work\n\n" +

              "🏠 A Simple Personal Security Policy:\n" +
              "• Use a unique strong password for every account — stored in a password manager\n" +
              "• Enable 2FA on all accounts that support it\n" +
              "• Back up important files every Sunday\n" +
              "• Install updates within 48 hours of release\n" +
              "• Never click links in unexpected emails without verifying the sender\n\n" +

              "🏢 Enforcing Policies in a Small Business or Family:\n" +
              "• Keep policies simple and specific — vague policies are ignored\n" +
              "• Use technical controls to enforce policies at the system level\n" +
              "• Lead by example — if management ignores the policy, everyone else will too\n\n" +

              "🌍 Real World Example:\n" +
              "The 2014 Sony Pictures hack exposed that Sony had no consistent password policy, used easily guessable passwords stored in unencrypted files and had no incident response plan. Unreleased movies, employee salary data and confidential emails were published online. Basic consistently-enforced policies would have significantly limited the damage.\n\n" +

              "🚨 Security Policy Checklist:\n" +
              "1. Write down your security rules — even one page is better than nothing\n" +
              "2. Review and update policies every six months\n" +
              "3. Ensure everyone knows and understands the policies\n" +
              "4. Test policies with simulated phishing exercises\n" +
              "5. Brief new employees or family members on policies from day one"
            }
        };
    }
}