using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SoulsFormats;

namespace EldenArmorPatcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class MainForm : Form
    {
        private TextBox txtGamePath;
        private TextBox txtModPath;
        private ComboBox cbHead, cbBody, cbArms, cbLegs;
        private ComboBox cbEditSlot;
        
        private string rawArmorData = @"
hd_m_1010.partsbnd.dcx - Iron Helmet
bd_m_1010.partsbnd.dcx - Scale Armor
am_m_1010.partsbnd.dcx - Iron Gauntlets
lg_m_1010.partsbnd.dcx - Leather Trousers
hd_m_1840.partsbnd.dcx - Kaiden Helm
bd_m_1840.partsbnd.dcx - Kaiden Armor
am_m_1840.partsbnd.dcx - Kaiden Gauntlets
lg_m_1840.partsbnd.dcx - Kaiden Trousers
hd_m_1100.partsbnd.dcx - Drake Knight Helm
bd_m_1100.partsbnd.dcx - Drake Knight Armor
am_m_1100.partsbnd.dcx - Drake Knight Gauntlets
lg_m_1100.partsbnd.dcx - Drake Knight Greaves
hd_m_1101.partsbnd.dcx - Drake Knight Helm (Altered)
bd_m_1101.partsbnd.dcx - Drake Knight Armor (Altered)
bd_f_1101.partsbnd.dcx - Drake Knight Armor (Altered)
hd_m_1120.partsbnd.dcx - Scaled Helm
bd_m_1120.partsbnd.dcx - Scaled Armor
am_m_1120.partsbnd.dcx - Scaled Gauntlets
lg_m_1120.partsbnd.dcx - Scaled Greaves
bd_m_1121.partsbnd.dcx - Scaled Armor (Altered)
hd_m_1130.partsbnd.dcx - Perfumer Hood
bd_m_1130.partsbnd.dcx - Perfumer Robe
am_m_1130.partsbnd.dcx - Perfumer Gloves
lg_m_1130.partsbnd.dcx - Perfumer Sarong
lg_f_1130.partsbnd.dcx - Perfumer Sarong
bd_m_1131.partsbnd.dcx - Perfumer Robe (Altered)
hd_m_1140.partsbnd.dcx - Traveler's Hat
bd_m_1140.partsbnd.dcx - Traveler's Traveling Garb
bd_m_1141.partsbnd.dcx - Traveler's Traveling Garb (Altered)
am_m_1140.partsbnd.dcx - Traveler's Gloves
lg_m_1140.partsbnd.dcx - Traveler's Slops
hd_m_1160.partsbnd.dcx - Alberich's Pointed Hat
bd_m_1160.partsbnd.dcx - Alberich's Robe
bd_f_1160.partsbnd.dcx - Alberich's Robe
am_m_1160.partsbnd.dcx - Alberich's Bracers
lg_m_1160.partsbnd.dcx - Alberich's Trousers
hd_m_1161.partsbnd.dcx - Alberich's Pointed Hat (Altered)
bd_m_1161.partsbnd.dcx - Alberich's Robe (Altered)
bd_f_1161.partsbnd.dcx - Alberich's Robe (Altered)
hd_m_1170.partsbnd.dcx - Spellblade's Pointed Hat
bd_m_1170.partsbnd.dcx - Spellblade's Traveling Attire
am_m_1170.partsbnd.dcx - Spellblade's Gloves
lg_m_1170.partsbnd.dcx - Spellblade's Trousers
bd_m_1171.partsbnd.dcx - Spellblade's Traveling Attire (Altered)
hd_m_1180.partsbnd.dcx - Bull-Goat Helm
bd_m_1180.partsbnd.dcx - Bull-Goat Armor
am_m_1180.partsbnd.dcx - Bull-Goat Gauntlets
lg_m_1180.partsbnd.dcx - Bull-Goat Greaves
hd_m_1190.partsbnd.dcx - Iron Kasa
bd_m_1190.partsbnd.dcx - Ronin's Armor
am_m_1190.partsbnd.dcx - Ronin's Gauntlets
lg_m_1190.partsbnd.dcx - Ronin's Greaves
bd_m_1191.partsbnd.dcx - Ronin's Armor (Altered)
hd_m_1200.partsbnd.dcx - Guilty Hood
bd_m_1200.partsbnd.dcx - Cloth Garb
bd_f_1200.partsbnd.dcx - Cloth Garb
lg_m_1200.partsbnd.dcx - Cloth Trousers
hd_m_1600.partsbnd.dcx - Black Wolf Mask
bd_m_1600.partsbnd.dcx - Blaidd's Armor
am_m_1600.partsbnd.dcx - Blaidd's Gauntlets
lg_m_1600.partsbnd.dcx - Blaidd's Greaves
bd_m_1601.partsbnd.dcx - Blaidd's Armor (Altered)
hd_m_1610.partsbnd.dcx - Black Knife Hood
bd_m_1610.partsbnd.dcx - Black Knife Armor
am_m_1610.partsbnd.dcx - Black Knife Gauntlets
lg_m_1610.partsbnd.dcx - Black Knife Greaves
bd_m_1611.partsbnd.dcx - Black Knife Armor (Altered)
hd_m_1640.partsbnd.dcx - Exile Hood
bd_m_1640.partsbnd.dcx - Exile Armor
am_m_1640.partsbnd.dcx - Exile Gauntlets
lg_m_1640.partsbnd.dcx - Exile Greaves
hd_m_1650.partsbnd.dcx - Banished Knight Helm
bd_m_1650.partsbnd.dcx - Banished Knight Armor
am_m_1650.partsbnd.dcx - Banished Knight Gauntlets
lg_m_1650.partsbnd.dcx - Banished Knight Greaves
hd_m_1651.partsbnd.dcx - Banished Knight Helm (Altered)
bd_m_1651.partsbnd.dcx - Banished Knight Armor (Altered)
bd_m_1652.partsbnd.dcx - Banished Knight Armor (Variant)
hd_m_1670.partsbnd.dcx - Briar Helm
bd_m_1670.partsbnd.dcx - Briar Armor
am_m_1670.partsbnd.dcx - Briar Gauntlets
lg_m_1670.partsbnd.dcx - Briar Greaves
bd_m_1671.partsbnd.dcx - Briar Armor (Altered)
hd_m_1680.partsbnd.dcx - Page Hood
bd_m_1680.partsbnd.dcx - Page Garb
lg_m_1680.partsbnd.dcx - Page Trousers
bd_m_1681.partsbnd.dcx - Page Garb (Altered)
hd_m_1690.partsbnd.dcx - Night's Cavalry Helm
bd_m_1690.partsbnd.dcx - Night's Cavalry Armor
am_m_1690.partsbnd.dcx - Night's Cavalry Gauntlets
lg_m_1690.partsbnd.dcx - Night's Cavalry Greaves
hd_m_1691.partsbnd.dcx - Night's Cavalry Helm (Altered)
bd_m_1691.partsbnd.dcx - Night's Cavalry Armor (Altered)
hd_m_1700.partsbnd.dcx - Blue Silver Mail Hood
bd_m_1700.partsbnd.dcx - Blue Silver Mail Armor
bd_f_1700.partsbnd.dcx - Blue Silver Mail Armor
am_m_1700.partsbnd.dcx - Blue Silver Bracelets
lg_m_1700.partsbnd.dcx - Blue Silver Mail Skirt
bd_m_1701.partsbnd.dcx - Blue Silver Mail Armor (Altered)
bd_f_1701.partsbnd.dcx - Blue Silver Mail Armor (Altered)
hd_m_1710.partsbnd.dcx - Nomadic Merchant's Chapeau
bd_m_1710.partsbnd.dcx - Nomadic Merchant's Finery
lg_m_1710.partsbnd.dcx - Nomadic Merchant's Trousers
bd_m_1711.partsbnd.dcx - Nomadic Merchant's Finery (Altered)
hd_m_1720.partsbnd.dcx - Malformed Dragon Helm
bd_m_1720.partsbnd.dcx - Malformed Dragon Armor
am_m_1720.partsbnd.dcx - Malformed Dragon Gauntlets
lg_m_1720.partsbnd.dcx - Malformed Dragon Greaves
hd_m_1730.partsbnd.dcx - Tree Sentinel Helm
bd_m_1730.partsbnd.dcx - Tree Sentinel Armor
am_m_1730.partsbnd.dcx - Tree Sentinel Gauntlets
lg_m_1730.partsbnd.dcx - Tree Sentinel Greaves
bd_m_1731.partsbnd.dcx - Tree Sentinel Armor (Altered)
hd_m_1740.partsbnd.dcx - Royal Knight Helm
bd_m_1740.partsbnd.dcx - Royal Knight Armor
am_m_1740.partsbnd.dcx - Royal Knight Gauntlets
lg_m_1740.partsbnd.dcx - Royal Knight Greaves
bd_m_1741.partsbnd.dcx - Royal Knight Armor (Altered)
hd_m_1750.partsbnd.dcx - Nox Monk Hood
hd_f_1750.partsbnd.dcx - Nox Monk Hood
bd_m_1750.partsbnd.dcx - Nox Monk Armor
bd_f_1750.partsbnd.dcx - Nox Monk Armor
am_m_1750.partsbnd.dcx - Nox Bracelets
lg_m_1750.partsbnd.dcx - Nox Greaves
lg_f_1750.partsbnd.dcx - Nox Greaves
hd_m_1751.partsbnd.dcx - Nox Monk Hood (Altered)
hd_f_1751.partsbnd.dcx - Nox Monk Hood (Altered)
bd_m_1751.partsbnd.dcx - Nox Monk Armor (Altered)
bd_f_1751.partsbnd.dcx - Nox Monk Armor (Altered)
hd_m_1755.partsbnd.dcx - Nox Swordstress Crown
bd_m_1755.partsbnd.dcx - Nox Swordstress Armor
bd_f_1755.partsbnd.dcx - Nox Swordstress Armor
hd_m_2630.partsbnd.dcx - Night Maiden Twin Crown
bd_m_2630.partsbnd.dcx - Night Maiden Armor
bd_f_2630.partsbnd.dcx - Night Maiden Armor
hd_m_1756.partsbnd.dcx - Nox Swordstress Crown (Altered)
bd_m_1756.partsbnd.dcx - Nox Swordstress Armor (Altered)
bd_f_1756.partsbnd.dcx - Nox Swordstress Armor (Altered)
hd_m_1760.partsbnd.dcx - Great Horned Headband
bd_m_1760.partsbnd.dcx - Fur Raiment
bd_f_1760.partsbnd.dcx - Fur Raiment
lg_m_1760.partsbnd.dcx - Fur Leggings
lg_f_1760.partsbnd.dcx - Fur Leggings
hd_m_1765.partsbnd.dcx - Shining Horned Headband
bd_m_1765.partsbnd.dcx - Shaman Furs
bd_f_1765.partsbnd.dcx - Shaman Furs
lg_m_1765.partsbnd.dcx - Shaman Leggings
lg_f_1765.partsbnd.dcx - Shaman Leggings
hd_m_1770.partsbnd.dcx - Duelist Helm
bd_m_1770.partsbnd.dcx - Gravekeeper Cloak
bd_f_1770.partsbnd.dcx - Gravekeeper Cloak
lg_m_1770.partsbnd.dcx - Duelist Greaves
bd_m_1771.partsbnd.dcx - Gravekeeper Cloak (Altered)
bd_f_1771.partsbnd.dcx - Gravekeeper Cloak (Altered)
hd_m_1790.partsbnd.dcx - Sanguine Noble Hood
bd_m_1790.partsbnd.dcx - Sanguine Noble Robe
lg_m_1790.partsbnd.dcx - Sanguine Noble Waistcloth
hd_m_1800.partsbnd.dcx - Guardian Mask
bd_m_1800.partsbnd.dcx - Guardian Garb (Full Bloom)
am_m_1800.partsbnd.dcx - Guardian Bracers
lg_m_1800.partsbnd.dcx - Guardian Greaves
bd_m_1801.partsbnd.dcx - Guardian Garb
hd_m_1810.partsbnd.dcx - Cleanrot Helm
bd_m_1810.partsbnd.dcx - Cleanrot Armor
am_m_1810.partsbnd.dcx - Cleanrot Gauntlets
lg_m_1810.partsbnd.dcx - Cleanrot Greaves
hd_m_1811.partsbnd.dcx - Cleanrot Helm (Altered)
bd_m_1811.partsbnd.dcx - Cleanrot Armor (Altered)
hd_m_1820.partsbnd.dcx - Fire Monk Hood
bd_m_1820.partsbnd.dcx - Fire Monk Armor
am_m_1820.partsbnd.dcx - Fire Monk Gauntlets
lg_m_1820.partsbnd.dcx - Fire Monk Greaves
hd_m_1825.partsbnd.dcx - Blackflame Monk Hood
bd_m_1825.partsbnd.dcx - Blackflame Monk Armor
am_m_1825.partsbnd.dcx - Blackflame Monk Gauntlets
lg_m_1825.partsbnd.dcx - Blackflame Monk Greaves
hd_m_1830.partsbnd.dcx - Fire Prelate Helm
bd_m_1830.partsbnd.dcx - Fire Prelate Armor
am_m_1830.partsbnd.dcx - Fire Prelate Gauntlets
lg_m_1830.partsbnd.dcx - Fire Prelate Greaves
bd_m_1831.partsbnd.dcx - Fire Prelate Armor (Altered)
hd_m_1860.partsbnd.dcx - Aristocrat Headband
bd_m_1860.partsbnd.dcx - Aristocrat Garb
bd_f_1860.partsbnd.dcx - Aristocrat Garb
lg_m_1860.partsbnd.dcx - Aristocrat Boots
bd_m_1861.partsbnd.dcx - Aristocrat Garb (Altered)
bd_f_1861.partsbnd.dcx - Aristocrat Garb (Altered)
hd_m_1870.partsbnd.dcx - Aristocrat Hat
bd_m_1870.partsbnd.dcx - Aristocrat Coat
hd_m_1880.partsbnd.dcx - Old Aristocrat Cowl
bd_m_1880.partsbnd.dcx - Old Aristocrat Gown
lg_m_1880.partsbnd.dcx - Old Aristocrat Shoes
lg_f_1880.partsbnd.dcx - Old Aristocrat Shoes
hd_m_1930.partsbnd.dcx - Vulgar Militia Helm
bd_m_1930.partsbnd.dcx - Vulgar Militia Armor
am_m_1930.partsbnd.dcx - Vulgar Militia Gauntlets
lg_m_1930.partsbnd.dcx - Vulgar Militia Greaves
hd_m_1940.partsbnd.dcx - Sage Hood
bd_m_1940.partsbnd.dcx - Sage Robe
lg_m_1940.partsbnd.dcx - Sage Trousers
hd_m_1950.partsbnd.dcx - Pumpkin Helm
hd_m_2000.partsbnd.dcx - Elden Lord Crown
bd_m_2000.partsbnd.dcx - Elden Lord Armor
am_m_2000.partsbnd.dcx - Elden Lord Bracers
lg_m_2000.partsbnd.dcx - Elden Lord Greaves
bd_m_2001.partsbnd.dcx - Elden Lord Armor (Altered)
hd_m_2010.partsbnd.dcx - Radahn's Redmane Helm
bd_m_2010.partsbnd.dcx - Radahn's Lion Armor
am_m_2010.partsbnd.dcx - Radahn's Gauntlets
lg_m_2010.partsbnd.dcx - Radahn's Greaves
bd_m_2011.partsbnd.dcx - Radahn's Lion Armor (Altered)
bd_m_2020.partsbnd.dcx - Lord of Blood's Robe
bd_m_2021.partsbnd.dcx - Lord of Blood's Robe (Altered)
hd_m_2050.partsbnd.dcx - Queen's Crescent Crown
bd_m_2050.partsbnd.dcx - Queen's Robe
am_m_2050.partsbnd.dcx - Queen's Bracelets
lg_m_2050.partsbnd.dcx - Queen's Leggings
hd_m_2060.partsbnd.dcx - Godskin Apostle Hood
bd_m_2060.partsbnd.dcx - Godskin Apostle Robe
am_m_2060.partsbnd.dcx - Godskin Apostle Bracelets
lg_m_2060.partsbnd.dcx - Godskin Apostle Trousers
hd_m_2070.partsbnd.dcx - Godskin Noble Hood
bd_m_2070.partsbnd.dcx - Godskin Noble Robe
am_m_2070.partsbnd.dcx - Godskin Noble Bracelets
lg_m_2070.partsbnd.dcx - Godskin Noble Trousers
hd_m_2080.partsbnd.dcx - Depraved Perfumer Headscarf
bd_m_2080.partsbnd.dcx - Depraved Perfumer Robe
am_m_2080.partsbnd.dcx - Depraved Perfumer Gloves
lg_m_2080.partsbnd.dcx - Depraved Perfumer Trousers
bd_m_2081.partsbnd.dcx - Depraved Perfumer Robe (Altered)
hd_m_2110.partsbnd.dcx - Crucible Axe Helm
bd_m_2110.partsbnd.dcx - Crucible Axe Armor
am_m_2110.partsbnd.dcx - Crucible Gauntlets
lg_m_2110.partsbnd.dcx - Crucible Greaves
hd_m_2115.partsbnd.dcx - Crucible Tree Helm
bd_m_2115.partsbnd.dcx - Crucible Tree Armor
bd_m_2111.partsbnd.dcx - Crucible Axe Armor (Altered)
bd_m_2116.partsbnd.dcx - Crucible Tree Armor (Altered)
hd_m_2120.partsbnd.dcx - Lusat's Glintstone Crown
bd_m_2120.partsbnd.dcx - Lusat's Robe
am_m_2120.partsbnd.dcx - Lusat's Manchettes
lg_m_2120.partsbnd.dcx - Old Sorcerer's Legwraps
hd_m_2125.partsbnd.dcx - Azur's Glintstone Crown
bd_m_2125.partsbnd.dcx - Azur's Glintstone Robe
am_m_2125.partsbnd.dcx - Azur's Manchettes
hd_m_1210.partsbnd.dcx - All-Knowing Helm
bd_m_1210.partsbnd.dcx - All-Knowing Armor
am_m_1210.partsbnd.dcx - All-Knowing Gauntlets
lg_m_1210.partsbnd.dcx - All-Knowing Greaves
bd_m_1211.partsbnd.dcx - All-Knowing Armor (Altered)
hd_m_1220.partsbnd.dcx - Twinned Helm
bd_m_1220.partsbnd.dcx - Twinned Armor
am_m_1220.partsbnd.dcx - Twinned Gauntlets
lg_m_1220.partsbnd.dcx - Twinned Greaves
bd_m_1221.partsbnd.dcx - Twinned Armor (Altered)
hd_m_1230.partsbnd.dcx - Ragged Hat
bd_m_1230.partsbnd.dcx - Ragged Armor
am_m_1230.partsbnd.dcx - Ragged Gloves
lg_m_1230.partsbnd.dcx - Ragged Loincloth
hd_m_1231.partsbnd.dcx - Ragged Hat (Altered)
bd_m_1231.partsbnd.dcx - Ragged Armor (Altered)
hd_m_1240.partsbnd.dcx - Prophet Blindfold
bd_m_1240.partsbnd.dcx - Corhyn's Robe
bd_f_1240.partsbnd.dcx - Corhyn's Robe
lg_m_1240.partsbnd.dcx - Prophet Trousers
bd_m_1241.partsbnd.dcx - Prophet Robe (Altered)
bd_f_1241.partsbnd.dcx - Prophet Robe (Altered)
bd_m_1245.partsbnd.dcx - Prophet Robe
bd_f_1245.partsbnd.dcx - Prophet Robe
hd_m_1250.partsbnd.dcx - Astrologer Hood
bd_m_1250.partsbnd.dcx - Astrologer Robe
bd_f_1250.partsbnd.dcx - Astrologer Robe
am_m_1250.partsbnd.dcx - Astrologer Gloves
lg_m_1250.partsbnd.dcx - Astrologer Trousers
bd_m_1251.partsbnd.dcx - Astrologer Robe (Altered)
bd_f_1251.partsbnd.dcx - Astrologer Robe (Altered)
hd_m_1260.partsbnd.dcx - Lionel's Helm
bd_m_1260.partsbnd.dcx - Lionel's Armor
am_m_1260.partsbnd.dcx - Lionel's Gauntlets
lg_m_1260.partsbnd.dcx - Lionel's Greaves
bd_m_1261.partsbnd.dcx - Lionel's Armor (Altered)
hd_m_1270.partsbnd.dcx - Hoslow's Helm
bd_m_1270.partsbnd.dcx - Hoslow's Armor
am_m_1270.partsbnd.dcx - Hoslow's Gauntlets
lg_m_1270.partsbnd.dcx - Hoslow's Greaves
hd_m_1275.partsbnd.dcx - Diallos's Mask
bd_m_1271.partsbnd.dcx - Hoslow's Armor (Altered)
hd_m_1280.partsbnd.dcx - Vagabond Knight Helm
bd_m_1280.partsbnd.dcx - Vagabond Knight Armor
am_m_1280.partsbnd.dcx - Vagabond Knight Gauntlets
lg_m_1280.partsbnd.dcx - Vagabond Knight Greaves
bd_m_1281.partsbnd.dcx - Vagabond Knight Armor (Altered)
hd_m_1290.partsbnd.dcx - Blue Cloth Cowl
bd_m_1290.partsbnd.dcx - Blue Cloth Vest
am_m_1290.partsbnd.dcx - Warrior Gauntlets
lg_m_1290.partsbnd.dcx - Warrior Greaves
hd_m_1300.partsbnd.dcx - White Mask
bd_m_1300.partsbnd.dcx - War Surgeon Gown
am_m_1300.partsbnd.dcx - War Surgeon Gloves
lg_m_1300.partsbnd.dcx - War Surgeon Trousers
bd_m_1301.partsbnd.dcx - War Surgeon Gown (Altered)
hd_m_1310.partsbnd.dcx - Royal Remains Helm
bd_m_1310.partsbnd.dcx - Royal Remains Armor
am_m_1310.partsbnd.dcx - Royal Remains Gauntlets
lg_m_1310.partsbnd.dcx - Royal Remains Greaves
hd_m_1320.partsbnd.dcx - Brave's Cord Circlet
bd_m_1320.partsbnd.dcx - Brave's Battlewear
bd_m_1321.partsbnd.dcx - Brave's Battlewear (Altered)
am_m_1320.partsbnd.dcx - Brave's Bracer
lg_m_1320.partsbnd.dcx - Brave's Legwraps
hd_m_1325.partsbnd.dcx - Brave's Leather Helm
hd_m_1321.partsbnd.dcx - Brave's Battlewear (Altered)
hd_m_1330.partsbnd.dcx - Beast Champion Helm
bd_m_1330.partsbnd.dcx - Beast Champion Armor
am_m_1330.partsbnd.dcx - Beast Champion Gauntlets
lg_m_1330.partsbnd.dcx - Beast Champion Greaves
bd_m_1331.partsbnd.dcx - Beast Champion Armor (Altered)
hd_m_1380.partsbnd.dcx - Champion Headband
bd_m_1380.partsbnd.dcx - Champion Pauldron
bd_f_1380.partsbnd.dcx - Champion Pauldron
am_m_1380.partsbnd.dcx - Champion Bracers
lg_m_1380.partsbnd.dcx - Champion Gaiters
lg_f_1380.partsbnd.dcx - Champion Gaiters
hd_m_1390.partsbnd.dcx - Crimson Hood
bd_m_1390.partsbnd.dcx - Noble's Traveling Garb
bd_f_1390.partsbnd.dcx - Noble's Traveling Garb
am_m_1390.partsbnd.dcx - Noble's Gloves
lg_m_1390.partsbnd.dcx - Noble's Trousers
hd_m_1395.partsbnd.dcx - Navy Hood
hd_m_1620.partsbnd.dcx - Maliketh's Helm
bd_m_1620.partsbnd.dcx - Maliketh's Armor
am_m_1620.partsbnd.dcx - Maliketh's Gauntlets
lg_m_1620.partsbnd.dcx - Maliketh's Greaves
bd_m_1621.partsbnd.dcx - Maliketh's Armor (Altered)
hd_m_1630.partsbnd.dcx - Malenia's Winged Helm
bd_m_1630.partsbnd.dcx - Malenia's Armor
bd_f_1630.partsbnd.dcx - Malenia's Armor
am_m_1630.partsbnd.dcx - Malenia's Gauntlet
lg_m_1630.partsbnd.dcx - Malenia's Greaves
bd_m_1631.partsbnd.dcx - Malenia's Armor (Altered)
bd_f_1631.partsbnd.dcx - Malenia's Armor (Altered)
hd_m_1660.partsbnd.dcx - Veteran's Helm
bd_m_1660.partsbnd.dcx - Veteran's Armor
am_m_1660.partsbnd.dcx - Veteran's Gauntlets
lg_m_1660.partsbnd.dcx - Veteran's Greaves
bd_m_1661.partsbnd.dcx - Veteran's Armor (Altered)
hd_m_1850.partsbnd.dcx - Bloodhound Knight Helm
bd_m_1850.partsbnd.dcx - Bloodhound Knight Armor
am_m_1850.partsbnd.dcx - Bloodhound Knight Gauntlets
lg_m_1850.partsbnd.dcx - Bloodhound Knight Greaves
bd_m_1851.partsbnd.dcx - Bloodhound Knight Armor (Altered)
hd_m_2130.partsbnd.dcx - Festive Hood
bd_m_2130.partsbnd.dcx - Festive Garb
bd_f_2130.partsbnd.dcx - Festive Garb
hd_m_2131.partsbnd.dcx - Festive Hood (Altered)
bd_m_2131.partsbnd.dcx - Festive Garb (Altered)
bd_f_2131.partsbnd.dcx - Festive Garb (Altered)
hd_m_2135.partsbnd.dcx - Blue Festive Hood
bd_m_2135.partsbnd.dcx - Blue Festive Garb
bd_f_2135.partsbnd.dcx - Blue Festive Garb
hd_m_2140.partsbnd.dcx - Commoner's Headband
bd_m_2140.partsbnd.dcx - Commoner's Garb
lg_m_2140.partsbnd.dcx - Commoner's Shoes
hd_m_2141.partsbnd.dcx - Commoner's Headband (Altered)
bd_m_2141.partsbnd.dcx - Commoner's Garb (Altered)
bd_m_2145.partsbnd.dcx - Commoner's Simple Garb
bd_m_2146.partsbnd.dcx - Commoner's Simple Garb (Altered)
hd_m_2150.partsbnd.dcx - Envoy Crown
hd_m_2160.partsbnd.dcx - Twinsage Glintstone Crown
bd_m_2160.partsbnd.dcx - Raya Lucarian Robe
bd_f_2160.partsbnd.dcx - Raya Lucarian Robe
am_m_2160.partsbnd.dcx - Sorcerer Manchettes
lg_m_2160.partsbnd.dcx - Sorcerer Leggings
lg_f_2160.partsbnd.dcx - Sorcerer Leggings
hd_m_2165.partsbnd.dcx - Olivinus Glintstone Crown
hd_m_2170.partsbnd.dcx - Lazuli Glintstone Crown
hd_m_2175.partsbnd.dcx - Karolos Glintstone Crown
hd_m_2540.partsbnd.dcx - Witch's Glintstone Crown
hd_m_2545.partsbnd.dcx - Sellen's Glintstone Crown
hd_m_2190.partsbnd.dcx - Marionette Soldier Helm
bd_m_2190.partsbnd.dcx - Marionette Soldier Armor
hd_m_2180.partsbnd.dcx - Marionette Soldier Birdhelm
hd_m_1350.partsbnd.dcx - Raging Wolf Helm
bd_m_1350.partsbnd.dcx - Raging Wolf Armor
am_m_1350.partsbnd.dcx - Raging Wolf Gauntlets
lg_m_1350.partsbnd.dcx - Raging Wolf Greaves
bd_m_1351.partsbnd.dcx - Raging Wolf Armor (Altered)
hd_m_1360.partsbnd.dcx - Land of Reeds Helm
bd_m_1360.partsbnd.dcx - Land of Reeds Armor
am_m_1360.partsbnd.dcx - Land of Reeds Gauntlets
lg_m_1360.partsbnd.dcx - Land of Reeds Greaves
bd_m_1361.partsbnd.dcx - Land of Reeds Armor (Altered)
hd_m_1365.partsbnd.dcx - Okina Mask
bd_m_1365.partsbnd.dcx - White Reed Armor
am_m_1365.partsbnd.dcx - White Reed Gauntlets
lg_m_1365.partsbnd.dcx - White Reed Greaves
hd_m_1400.partsbnd.dcx - Confessor Hood
bd_m_1400.partsbnd.dcx - Confessor Armor
am_m_1400.partsbnd.dcx - Confessor Gloves
lg_m_1400.partsbnd.dcx - Confessor Boots
hd_m_1401.partsbnd.dcx - Confessor Hood (Altered)
bd_m_1401.partsbnd.dcx - Confessor Armor (Altered)
hd_m_1410.partsbnd.dcx - Prisoner Iron Mask
bd_m_1410.partsbnd.dcx - Prisoner Clothing
bd_f_1410.partsbnd.dcx - Prisoner Clothing
bd_m_1411.partsbnd.dcx - Prisoner Clothing (Variant)
lg_m_1410.partsbnd.dcx - Prisoner Trousers
hd_m_1415.partsbnd.dcx - Blackguard's Iron Mask
hd_m_1420.partsbnd.dcx - Traveling Maiden Hood
bd_m_1420.partsbnd.dcx - Traveling Maiden Robe
bd_f_1420.partsbnd.dcx - Traveling Maiden Robe
bd_m_1422.partsbnd.dcx - Traveling Maiden Robe (Variant)
bd_f_1422.partsbnd.dcx - Traveling Maiden Robe (Variant)
bd_f_1423.partsbnd.dcx - Bloodied Traveling Maiden Robe
am_m_1420.partsbnd.dcx - Traveling Maiden Gloves
lg_m_1420.partsbnd.dcx - Traveling Maiden Boots
bd_m_1421.partsbnd.dcx - Traveling Maiden Robe (Altered)
bd_f_1421.partsbnd.dcx - Traveling Maiden Robe (Altered)
hd_m_1425.partsbnd.dcx - Finger Maiden Fillet
bd_m_1425.partsbnd.dcx - Finger Maiden Robe
bd_f_1425.partsbnd.dcx - Finger Maiden Robe
lg_m_1425.partsbnd.dcx - Finger Maiden Shoes
lg_f_1425.partsbnd.dcx - Finger Maiden Shoes
bd_m_1426.partsbnd.dcx - Finger Maiden Robe (Altered)
bd_f_1426.partsbnd.dcx - Finger Maiden Robe (Altered)
bd_f_1427.partsbnd.dcx - Bloodied Finger Maiden Robe
hd_m_1430.partsbnd.dcx - Preceptor's Big Hat
bd_m_1430.partsbnd.dcx - Preceptor's Long Gown
am_m_1430.partsbnd.dcx - Preceptor's Gloves
lg_m_1430.partsbnd.dcx - Preceptor's Trousers
hd_m_1431.partsbnd.dcx - Mask of Confidence
bd_m_1431.partsbnd.dcx - Preceptor's Long Gown (Altered)
hd_m_1440.partsbnd.dcx - Grass Hair Ornament
hd_m_1450.partsbnd.dcx - Skeletal Mask
bd_m_1450.partsbnd.dcx - Raptor's Black Feathers
am_m_1450.partsbnd.dcx - Bandit Manchettes
lg_m_1450.partsbnd.dcx - Bandit Boots
bd_m_1451.partsbnd.dcx - Bandit Garb
bd_f_1451.partsbnd.dcx - Bandit Garb
hd_m_1460.partsbnd.dcx - Eccentric's Hood
bd_m_1460.partsbnd.dcx - Eccentric's Armor
am_m_1460.partsbnd.dcx - Eccentric's Manchettes
lg_m_1460.partsbnd.dcx - Eccentric's Breeches
hd_m_1461.partsbnd.dcx - Eccentric's Hood (Altered)
hd_m_1470.partsbnd.dcx - Fingerprint Helm
bd_m_1470.partsbnd.dcx - Fingerprint Armor
am_m_1470.partsbnd.dcx - Fingerprint Gauntlets
lg_m_1470.partsbnd.dcx - Fingerprint Greaves
bd_m_1471.partsbnd.dcx - Fingerprint Armor (Altered)
hd_m_1480.partsbnd.dcx - Consort's Mask
bd_m_1480.partsbnd.dcx - Consort's Robe
bd_f_1480.partsbnd.dcx - Consort's Robe
lg_m_1480.partsbnd.dcx - Consort's Trousers
hd_m_1530.partsbnd.dcx - Ruler's Mask
bd_m_1530.partsbnd.dcx - Ruler's Robe
bd_f_1530.partsbnd.dcx - Ruler's Robe
bd_m_1531.partsbnd.dcx - Upper-Class Robe
bd_f_1531.partsbnd.dcx - Upper-Class Robe
hd_m_1485.partsbnd.dcx - Marais Mask
bd_m_1485.partsbnd.dcx - Marais Robe
am_m_1485.partsbnd.dcx - Bloodsoaked Manchettes
hd_m_1486.partsbnd.dcx - Bloodsoaked Mask
bd_m_1486.partsbnd.dcx - Official's Attire
bd_f_1486.partsbnd.dcx - Official's Attire
hd_m_1490.partsbnd.dcx - Omen Helm
bd_m_1490.partsbnd.dcx - Omen Armor
am_m_1490.partsbnd.dcx - Omen Gauntlets
lg_m_1490.partsbnd.dcx - Omen Greaves
hd_m_1500.partsbnd.dcx - Carian Knight Helm
bd_m_1500.partsbnd.dcx - Carian Knight Armor
am_m_1500.partsbnd.dcx - Carian Knight Gauntlets
lg_m_1500.partsbnd.dcx - Carian Knight Greaves
bd_m_1501.partsbnd.dcx - Carian Knight Armor (Altered)
hd_m_1510.partsbnd.dcx - Hierodas Glintstone Crown
bd_m_1510.partsbnd.dcx - Errant Sorcerer Robe
am_m_1510.partsbnd.dcx - Errant Sorcerer Manchettes
lg_m_1510.partsbnd.dcx - Errant Sorcerer Boots
bd_m_1511.partsbnd.dcx - Errant Sorcerer Robe (Altered)
hd_m_1520.partsbnd.dcx - Haima Glintstone Crown
bd_m_1520.partsbnd.dcx - Battlemage Robe
am_m_1520.partsbnd.dcx - Battlemage Manchettes
lg_m_1520.partsbnd.dcx - Battlemage Legwraps
hd_m_2200.partsbnd.dcx - Snow Witch Hat
bd_m_2200.partsbnd.dcx - Snow Witch Robe
bd_f_2200.partsbnd.dcx - Snow Witch Robe
lg_m_2200.partsbnd.dcx - Snow Witch Skirt
bd_m_2201.partsbnd.dcx - Snow Witch Robe (Altered)
bd_f_2201.partsbnd.dcx - Snow Witch Robe (Altered)
bd_m_2210.partsbnd.dcx - Traveler's Clothes
bd_f_2210.partsbnd.dcx - Traveler's Clothes
am_m_2210.partsbnd.dcx - Traveler's Manchettes
lg_m_2210.partsbnd.dcx - Traveler's Boots
hd_m_2220.partsbnd.dcx - Juvenile Scholar Cap
bd_m_2220.partsbnd.dcx - Juvenile Scholar Robe
hd_m_2230.partsbnd.dcx - Radiant Gold Mask
hd_f_2230.partsbnd.dcx - Radiant Gold Mask
bd_m_2230.partsbnd.dcx - Goldmask's Rags
bd_f_2230.partsbnd.dcx - Goldmask's Rags
am_m_2230.partsbnd.dcx - Gold Bracelets
lg_m_2230.partsbnd.dcx - Gold Waistwrap
lg_f_2230.partsbnd.dcx - Gold Waistwrap
bd_m_2240.partsbnd.dcx - Fell Omen Cloak
bd_f_2240.partsbnd.dcx - Fell Omen Cloak
hd_m_2250.partsbnd.dcx - Albinauric Mask
bd_m_2250.partsbnd.dcx - Dirty Chainmail
hd_m_2260.partsbnd.dcx - Zamor Mask
bd_m_2260.partsbnd.dcx - Zamor Armor
am_m_2260.partsbnd.dcx - Zamor Bracelets
lg_m_2260.partsbnd.dcx - Zamor Legwraps
hd_m_2270.partsbnd.dcx - Imp Head (Cat)
hd_m_2510.partsbnd.dcx - Imp Head (Fanged)
hd_m_2520.partsbnd.dcx - Imp Head (Long-Tongued)
hd_m_2570.partsbnd.dcx - Imp Head (Corpse)
hd_m_2575.partsbnd.dcx - Imp Head (Wolf)
hd_m_2580.partsbnd.dcx - Imp Head (Elder)
hd_m_2280.partsbnd.dcx - Silver Tear Mask
hd_m_1540.partsbnd.dcx - Chain Coif
hd_f_1540.partsbnd.dcx - Chain Coif
bd_m_1540.partsbnd.dcx - Chain Armor
bd_f_1540.partsbnd.dcx - Chain Armor
am_m_1540.partsbnd.dcx - Chain Gauntlets
am_f_1540.partsbnd.dcx - Chain Gauntlets
lg_m_1540.partsbnd.dcx - Chain Leggings
hd_m_1545.partsbnd.dcx - Greathelm
bd_m_1545.partsbnd.dcx - Eye Surcoat
bd_m_1546.partsbnd.dcx - Tree Surcoat
hd_m_2290.partsbnd.dcx - Octopus Head
hd_m_2300.partsbnd.dcx - Jar
hd_m_2310.partsbnd.dcx - Mushroom Head
bd_m_2310.partsbnd.dcx - Mushroom Body
bd_f_2310.partsbnd.dcx - Mushroom Body
am_m_2310.partsbnd.dcx - Mushroom Arms
am_f_2310.partsbnd.dcx - Mushroom Arms
lg_m_2310.partsbnd.dcx - Mushroom Legs
lg_f_2310.partsbnd.dcx - Mushroom Legs
hd_m_2610.partsbnd.dcx - Mushroom Crown
hd_m_1550.partsbnd.dcx - Nox Mirrorhelm
hd_m_1555.partsbnd.dcx - Iji's Mirrorhelm
hd_m_1560.partsbnd.dcx - Black Hood
bd_m_1560.partsbnd.dcx - Leather Armor
bd_f_1560.partsbnd.dcx - Leather Armor
am_m_1560.partsbnd.dcx - Leather Gloves
lg_m_1560.partsbnd.dcx - Leather Boots
hd_m_1561.partsbnd.dcx - Bandit Mask
hd_m_1570.partsbnd.dcx - Knight Helm
bd_m_1570.partsbnd.dcx - Knight Armor
am_m_1570.partsbnd.dcx - Knight Gauntlets
lg_m_1570.partsbnd.dcx - Knight Greaves
hd_m_1580.partsbnd.dcx - Greathood
hd_m_2320.partsbnd.dcx - Godrick Soldier Helm
bd_m_2320.partsbnd.dcx - Tree-and-Beast Surcoat
am_m_2320.partsbnd.dcx - Godrick Soldier Gauntlets
lg_m_2320.partsbnd.dcx - Godrick Soldier Greaves
hd_m_2330.partsbnd.dcx - Raya Lucarian Helm
bd_m_2330.partsbnd.dcx - Cuckoo Surcoat
am_m_2330.partsbnd.dcx - Raya Lucarian Gauntlets
lg_m_2330.partsbnd.dcx - Raya Lucarian Greaves
hd_m_2340.partsbnd.dcx - Leyndell Soldier Helm
bd_m_2340.partsbnd.dcx - Erdtree Surcoat
am_m_2340.partsbnd.dcx - Leyndell Soldier Gauntlets
lg_m_2340.partsbnd.dcx - Leyndell Soldier Greaves
hd_m_2350.partsbnd.dcx - Radahn Soldier Helm
bd_m_2350.partsbnd.dcx - Redmane Surcoat
am_m_2350.partsbnd.dcx - Radahn Soldier Gauntlets
lg_m_2350.partsbnd.dcx - Radahn Soldier Greaves
bd_m_2360.partsbnd.dcx - Mausoleum Surcoat
am_m_2360.partsbnd.dcx - Mausoleum Gauntlets
lg_m_2360.partsbnd.dcx - Mausoleum Greaves
hd_m_2370.partsbnd.dcx - Haligtree Helm
bd_m_2370.partsbnd.dcx - Haligtree Crest Surcoat
am_m_2370.partsbnd.dcx - Haligtree Gauntlets
lg_m_2370.partsbnd.dcx - Haligtree Greaves
hd_m_2380.partsbnd.dcx - Gelmir Knight Helm
bd_m_2380.partsbnd.dcx - Gelmir Knight Armor
am_m_2380.partsbnd.dcx - Gelmir Knight Gauntlets
lg_m_2380.partsbnd.dcx - Gelmir Knight Greaves
bd_m_2381.partsbnd.dcx - Gelmir Knight Armor (Altered)
hd_m_2390.partsbnd.dcx - Godrick Knight Helm
bd_m_2390.partsbnd.dcx - Godrick Knight Armor
am_m_2390.partsbnd.dcx - Godrick Knight Gauntlets
lg_m_2390.partsbnd.dcx - Godrick Knight Greaves
bd_m_2391.partsbnd.dcx - Godrick Knight Armor (Altered)
hd_m_2400.partsbnd.dcx - Cuckoo Knight Helm
bd_m_2400.partsbnd.dcx - Cuckoo Knight Armor
am_m_2400.partsbnd.dcx - Cuckoo Knight Gauntlets
lg_m_2400.partsbnd.dcx - Cuckoo Knight Greaves
bd_m_2401.partsbnd.dcx - Cuckoo Knight Armor (Altered)
hd_m_2410.partsbnd.dcx - Leyndell Knight Helm
bd_m_2410.partsbnd.dcx - Leyndell Knight Armor
am_m_2410.partsbnd.dcx - Leyndell Knight Gauntlets
lg_m_2410.partsbnd.dcx - Leyndell Knight Greaves
bd_m_2411.partsbnd.dcx - Leyndell Knight Armor (Altered)
hd_m_2420.partsbnd.dcx - Redmane Knight Helm
bd_m_2420.partsbnd.dcx - Redmane Knight Armor
am_m_2420.partsbnd.dcx - Redmane Knight Gauntlets
lg_m_2420.partsbnd.dcx - Redmane Knight Greaves
bd_m_2421.partsbnd.dcx - Redmane Knight Armor (Altered)
bd_m_2430.partsbnd.dcx - Mausoleum Knight Armor
am_m_2430.partsbnd.dcx - Mausoleum Knight Gauntlets
lg_m_2430.partsbnd.dcx - Mausoleum Knight Greaves
bd_m_2431.partsbnd.dcx - Mausoleum Knight Armor (Altered)
hd_m_2440.partsbnd.dcx - Haligtree Knight Helm
bd_m_2440.partsbnd.dcx - Haligtree Knight Armor
am_m_2440.partsbnd.dcx - Haligtree Knight Gauntlets
lg_m_2440.partsbnd.dcx - Haligtree Knight Greaves
bd_m_2441.partsbnd.dcx - Haligtree Knight Armor (Altered)
hd_m_2450.partsbnd.dcx - Foot Soldier Cap
bd_m_2450.partsbnd.dcx - Chain-Draped Tabard
am_m_2450.partsbnd.dcx - Foot Soldier Gauntlets
lg_m_2450.partsbnd.dcx - Foot Soldier Greaves
hd_m_2460.partsbnd.dcx - Foot Soldier Helmet
bd_m_2460.partsbnd.dcx - Foot Soldier Tabard
hd_m_2470.partsbnd.dcx - Gilded Foot Soldier Cap
bd_m_2470.partsbnd.dcx - Leather-Draped Tabard
hd_m_2480.partsbnd.dcx - Foot Soldier Helm
bd_m_2480.partsbnd.dcx - Scarlet Tabard
bd_m_2490.partsbnd.dcx - Bloodsoaked Tabard
hd_m_2500.partsbnd.dcx - Sacred Crown Helm
bd_m_2500.partsbnd.dcx - Ivory-Draped Tabard
bd_f_2500.partsbnd.dcx - Ivory-Draped Tabard
hd_m_2530.partsbnd.dcx - Omensmirk Mask
bd_m_2530.partsbnd.dcx - Omenkiller Robe
am_m_2530.partsbnd.dcx - Omenkiller Long Gloves
lg_m_2530.partsbnd.dcx - Omenkiller Boots
hd_m_2550.partsbnd.dcx - Ash-of-War Scarab
hd_m_2551.partsbnd.dcx - Incantation Scarab
hd_m_2552.partsbnd.dcx - Glintstone Scarab
hd_m_2555.partsbnd.dcx - Crimson Tear Scarab
hd_m_2560.partsbnd.dcx - Cerulean Tear Scarab
bd_m_1590.partsbnd.dcx - Deathbed Dress
bd_f_1590.partsbnd.dcx - Deathbed Dress
lg_m_1590.partsbnd.dcx - Deathbed Smalls
lg_f_1590.partsbnd.dcx - Deathbed Smalls
hd_m_1595.partsbnd.dcx - Fia's Hood
bd_m_1595.partsbnd.dcx - Fia's Robe
bd_f_1595.partsbnd.dcx - Fia's Robe
bd_f_1597.partsbnd.dcx - Bloodied Fia's Robe
bd_m_1596.partsbnd.dcx - Fia's Robe (Altered)
bd_f_1596.partsbnd.dcx - Fia's Robe (Altered)
bd_m_2590.partsbnd.dcx - Millicent's Robe
bd_f_2590.partsbnd.dcx - Millicent's Robe
am_m_2590.partsbnd.dcx - Millicent's Gloves
am_f_2595.partsbnd.dcx - Millicent's Glove
am_f_2596.partsbnd.dcx - Millicent's Gloves
lg_m_2590.partsbnd.dcx - Millicent's Boots
bd_m_2600.partsbnd.dcx - Millicent's Tunic
bd_f_2595.partsbnd.dcx - Melina's Bloodied Robe
bd_f_2596.partsbnd.dcx - Millicent's Robe
bd_f_2600.partsbnd.dcx - Millicent's Tunic
am_m_2600.partsbnd.dcx - Golden Prosthetic
hd_m_2505.partsbnd.dcx - Highwayman Hood
bd_m_2505.partsbnd.dcx - Highwayman Cloth Armor
am_m_2505.partsbnd.dcx - Highwayman Gauntlets
hd_m_1685.partsbnd.dcx - High Page Hood
bd_m_1685.partsbnd.dcx - High Page Clothes
bd_m_1686.partsbnd.dcx - High Page Clothes (Altered)
hd_m_1775.partsbnd.dcx - Rotten Duelist Helm
bd_m_1775.partsbnd.dcx - Rotten Gravekeeper Cloak
bd_f_1775.partsbnd.dcx - Rotten Gravekeeper Cloak
lg_m_1775.partsbnd.dcx - Rotten Duelist Greaves
bd_m_1776.partsbnd.dcx - Rotten Gravekeeper Cloak (Altered)
bd_f_1776.partsbnd.dcx - Rotten Gravekeeper Cloak (Altered)
hd_m_2620.partsbnd.dcx - Black Dumpling
bd_m_2640.partsbnd.dcx - Lazuli Robe
bd_f_2640.partsbnd.dcx - Lazuli Robe
hd_m_2700.partsbnd.dcx - Dane's Hat
bd_m_2700.partsbnd.dcx - Dryleaf Robe
am_m_2700.partsbnd.dcx - Dryleaf Arm Wraps
lg_m_2700.partsbnd.dcx - Dryleaf Cuissardes
bd_m_2701.partsbnd.dcx - Dryleaf Robe (Altered)
hd_m_3000.partsbnd.dcx - Gaius's Helm
bd_m_3000.partsbnd.dcx - Gaius's Armor
am_m_3000.partsbnd.dcx - Gaius's Gauntlets
lg_m_3000.partsbnd.dcx - Gaius's Greaves
hd_m_2730.partsbnd.dcx - Oathseeker Knight Helm
bd_m_2730.partsbnd.dcx - Leda's Armor
bd_f_2730.partsbnd.dcx - Leda's Armor
am_m_2730.partsbnd.dcx - Oathseeker Knight Gauntlets
lg_m_2730.partsbnd.dcx - Oathseeker Knight Greaves
bd_m_2735.partsbnd.dcx - Oathseeker Knight Armor
hd_m_2710.partsbnd.dcx - Verdigris Helm
bd_m_2710.partsbnd.dcx - Verdigris Armor
am_m_2710.partsbnd.dcx - Verdigris Gauntlets
lg_m_2710.partsbnd.dcx - Verdigris Greaves
hd_m_2720.partsbnd.dcx - Pelt of Ralva
bd_m_2720.partsbnd.dcx - Iron Rivet Armor
bd_f_2720.partsbnd.dcx - Iron Rivet Armor
am_m_2720.partsbnd.dcx - Iron Rivet Gauntlets
lg_m_2720.partsbnd.dcx - Iron Rivet Greaves
hd_m_2721.partsbnd.dcx - Fang Helm
hd_m_2750.partsbnd.dcx - Thiollier's Mask
bd_m_2750.partsbnd.dcx - Thiollier's Garb
am_m_2750.partsbnd.dcx - Thiollier's Gloves
lg_m_2750.partsbnd.dcx - Thiollier's Trousers
bd_m_2751.partsbnd.dcx - Thiollier's Garb (Altered)
hd_m_2740.partsbnd.dcx - Dragon-form (Empty)
bd_m_2740.partsbnd.dcx - Dragon-form
am_m_2740.partsbnd.dcx - Dragon-form (Empty)
lg_m_2740.partsbnd.dcx - Dragon-form (Empty)
bd_m_2745.partsbnd.dcx - Dragon-form with Red Armor
hd_m_2770.partsbnd.dcx - High Priest Hat
bd_m_2770.partsbnd.dcx - High Priest Robe
bd_f_2770.partsbnd.dcx - High Priest Robe
am_m_2770.partsbnd.dcx - High Priest Gloves
lg_m_2770.partsbnd.dcx - High Priest Undergarments
bd_m_2775.partsbnd.dcx - Finger Robe
hd_m_2790.partsbnd.dcx - Caterpillar Mask
bd_m_2790.partsbnd.dcx - Braided Cord Robe
bd_f_2790.partsbnd.dcx - Braided Cord Robe
am_m_2790.partsbnd.dcx - Braided Arm Wraps
lg_m_2790.partsbnd.dcx - Soiled Loincloth
hd_m_2760.partsbnd.dcx - Dancer's Hood
bd_m_2760.partsbnd.dcx - Dancer's Dress
bd_f_2760.partsbnd.dcx - Dancer's Dress
am_m_2760.partsbnd.dcx - Dancer's Bracer
lg_m_2760.partsbnd.dcx - Dancer's Trousers
lg_f_2760.partsbnd.dcx - Dancer's Trousers
bd_m_2761.partsbnd.dcx - Dancer's Dress (Altered)
bd_f_2761.partsbnd.dcx - Dancer's Dress (Altered)
hd_m_2780.partsbnd.dcx - Helm of Night
bd_m_2780.partsbnd.dcx - Armor of Night
am_m_2780.partsbnd.dcx - Gauntlets of Night
lg_m_2780.partsbnd.dcx - Greaves of Night
hd_m_2850.partsbnd.dcx - Igon's Helm
bd_m_2850.partsbnd.dcx - Igon's Armor
am_m_2850.partsbnd.dcx - Igon's Gauntlets
lg_m_2850.partsbnd.dcx - Igon's Loincloth
hd_m_2851.partsbnd.dcx - Igon's Helm (Altered)
bd_m_2851.partsbnd.dcx - Igon's Armor (Altered)
bd_m_2852.partsbnd.dcx - Unknown - Ragged Armor
lg_m_2852.partsbnd.dcx - Unknown - Tunic
hd_m_2800.partsbnd.dcx - Wise Man's Mask
bd_m_2800.partsbnd.dcx - Ansbach's Attire
am_m_2800.partsbnd.dcx - Ansbach's Manchettes
lg_m_2800.partsbnd.dcx - Ansbach's Boots
bd_m_2801.partsbnd.dcx - Ansbach's Attire (Altered)
hd_m_2810.partsbnd.dcx - Freyja's Helm
bd_m_2810.partsbnd.dcx - Freyja's Armor
bd_f_2810.partsbnd.dcx - Freyja's Armor
am_m_2810.partsbnd.dcx - Freyja's Gauntlets
lg_m_2810.partsbnd.dcx - Freyja's Greaves
lg_f_2810.partsbnd.dcx - Freyja's Greaves
bd_m_2811.partsbnd.dcx - Freyja's Armor (Altered)
bd_f_2811.partsbnd.dcx - Freyja's Armor (Altered)
hd_m_2815.partsbnd.dcx - Freyja's Helm (Hair Forward)
hd_m_2820.partsbnd.dcx - Helm of Solitude
bd_m_2820.partsbnd.dcx - Armor of Solitude
am_m_2820.partsbnd.dcx - Gauntlets of Solitude
lg_m_2820.partsbnd.dcx - Greaves of Solitude
bd_m_2821.partsbnd.dcx - Armor of Solitude (Altered)
hd_m_3050.partsbnd.dcx - Messmer Soldier Helm
bd_m_3050.partsbnd.dcx - Messmer Soldier Armor
am_m_3050.partsbnd.dcx - Messmer Soldier Gauntlets
lg_m_3050.partsbnd.dcx - Messmer Soldier Greaves
bd_m_3051.partsbnd.dcx - Messmer Soldier Armor (Altered)
hd_m_3060.partsbnd.dcx - Black Knight Helm
bd_m_3060.partsbnd.dcx - Black Knight Armor
am_m_3060.partsbnd.dcx - Black Knight Gauntlets
lg_m_3060.partsbnd.dcx - Black Knight Greaves
hd_m_2830.partsbnd.dcx - Rakshasa Helm
bd_m_2830.partsbnd.dcx - Rakshasa Armor
am_m_2830.partsbnd.dcx - Rakshasa Gauntlets
lg_m_2830.partsbnd.dcx - Rakshasa Greaves
hd_m_2840.partsbnd.dcx - Lamenter-form (Empty)
bd_m_2840.partsbnd.dcx - Lamenter-form
am_m_2840.partsbnd.dcx - Lamenter-form (Empty)
lg_m_2840.partsbnd.dcx - Lamenter-form (Empty)
hd_m_3100.partsbnd.dcx - Fire Knight Helm
bd_m_3100.partsbnd.dcx - Fire Knight Armor
am_m_3100.partsbnd.dcx - Fire Knight Gauntlets
lg_m_3100.partsbnd.dcx - Fire Knight Greaves
bd_m_3101.partsbnd.dcx - Fire Knight Armor (Altered)
hd_m_3105.partsbnd.dcx - Death Mask Helm
hd_m_3106.partsbnd.dcx - Winged Serpent Helm
hd_m_3107.partsbnd.dcx - Salza's Hood
hd_m_2860.partsbnd.dcx - Leather Headband
bd_m_2860.partsbnd.dcx - Gloried Attire
am_m_2860.partsbnd.dcx - Leather Arm Wraps
lg_m_2860.partsbnd.dcx - Leather Leg Wraps
hd_m_2861.partsbnd.dcx - Leather Crown
bd_m_2861.partsbnd.dcx - Highland Attire
hd_m_3010.partsbnd.dcx - Death Knight Helm
bd_m_3010.partsbnd.dcx - Death Knight Armor
am_m_3010.partsbnd.dcx - Death Knight Gauntlets
lg_m_3010.partsbnd.dcx - Death Knight Greaves
hd_m_3020.partsbnd.dcx - Curseblade Mask
bd_m_3020.partsbnd.dcx - Ascetic's Loincloth
bd_f_3020.partsbnd.dcx - Ascetic's Loincloth
am_m_3020.partsbnd.dcx - Ascetic's Wrist Guards
lg_m_3020.partsbnd.dcx - Ascetic's Ankle Guards
hd_m_3030.partsbnd.dcx - Messmer's Helm
bd_m_3030.partsbnd.dcx - Messmer's Armor
bd_f_3030.partsbnd.dcx - Messmer's Armor
am_m_3030.partsbnd.dcx - Messmer's Gauntlets
lg_m_3030.partsbnd.dcx - Messmer's Greaves
lg_f_3030.partsbnd.dcx - Messmer's Greaves
hd_m_3031.partsbnd.dcx - Messmer's Helm (Altered)
hd_m_3040.partsbnd.dcx - Gravebird Helm
bd_m_3040.partsbnd.dcx - Gravebird's Blackquill Armor
bd_f_3040.partsbnd.dcx - Gravebird's Blackquill Armor
am_m_3040.partsbnd.dcx - Gravebird Bracelets
lg_m_3040.partsbnd.dcx - Gravebird Anklets
lg_f_3040.partsbnd.dcx - Gravebird Anklets
bd_m_3041.partsbnd.dcx - Gravebird Armor
bd_f_3041.partsbnd.dcx - Gravebird Armor
hd_m_3070.partsbnd.dcx - Common Soldier Helm
bd_m_3070.partsbnd.dcx - Common Soldier Cloth Armor
am_m_3070.partsbnd.dcx - Common Soldier Gauntlets
lg_m_3070.partsbnd.dcx - Common Soldier Greaves
hd_m_3080.partsbnd.dcx - Horned Warrior Helm
bd_m_3080.partsbnd.dcx - Horned Warrior Armor
am_m_3080.partsbnd.dcx - Horned Warrior Gauntlets
lg_m_3080.partsbnd.dcx - Horned Warrior Greaves
hd_m_3085.partsbnd.dcx - Divine Beast Helm
bd_m_3085.partsbnd.dcx - Divine Beast Warrior Armor
hd_m_3086.partsbnd.dcx - Divine Bird Helm
bd_m_3086.partsbnd.dcx - Divine Bird Warrior Armor
am_m_3086.partsbnd.dcx - Divine Bird Warrior Gauntlets
lg_m_3086.partsbnd.dcx - Divine Bird Warrior Greaves
hd_m_3090.partsbnd.dcx - Rellana's Helm
bd_m_3090.partsbnd.dcx - Rellana's Armor
am_m_3090.partsbnd.dcx - Rellana's Gloves
lg_m_3090.partsbnd.dcx - Rellana's Greaves
hd_m_3110.partsbnd.dcx - Young Lion's Helm
bd_m_3110.partsbnd.dcx - Young Lion's Armor
bd_m_3111.partsbnd.dcx - Young Lion's Armor (Altered)
am_m_3110.partsbnd.dcx - Young Lion's Gauntlets
lg_m_3110.partsbnd.dcx - Young Lion's Greaves
hd_m_3120.partsbnd.dcx - Circlet of Light
hd_m_3130.partsbnd.dcx - Shadow Militiaman Helm
bd_m_3130.partsbnd.dcx - Shadow Militiaman Armor
am_m_3130.partsbnd.dcx - Shadow Militiaman Gauntlets
lg_m_3130.partsbnd.dcx - Shadow Militiaman Greaves
hd_m_3140.partsbnd.dcx - Divine Beast Head
hd_m_2870.partsbnd.dcx - St. Trina's Blossom
hd_m_3150.partsbnd.dcx - Crucible Hammer-Helm
hd_m_3160.partsbnd.dcx - Greatjar
hd_m_3170.partsbnd.dcx - Imp Head (Lion)
bd_m_2755.partsbnd.dcx - Unused Armor?

";

        private List<ArmorItem> armorList = new List<ArmorItem>();

        public MainForm()
        {
            this.Text = "Elden Ring Armor Patcher";
            this.Size = new Size(600, 520);
            this.StartPosition = FormStartPosition.CenterScreen;

            ParseArmorData();
            InitializeUI();
        }

        private void ParseArmorData()
        {
            armorList.Add(new ArmorItem { ID = 0, Name = "-- SKIP --", SortName = "", Part = "all" });

            var lines = rawArmorData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var match = Regex.Match(line, @"([a-z]{2})_[a-z]_(\d{4}).*?-\s*(.*)");
                if (match.Success)
                {
                    string armorName = match.Groups[3].Value.Trim();
                    armorList.Add(new ArmorItem
                    {
                        Part = match.Groups[1].Value,
                        ID = int.Parse(match.Groups[2].Value),
                        Name = $"[{match.Groups[2].Value}] {armorName}",
                        SortName = armorName 
                    });
                }
            }
        }

        private void InitializeUI()
        {
            int y = 20;

            Label lblGame = new Label { Text = "Game folder (where eldenring.exe is located):", Location = new Point(20, y), Width = 300 };
            this.Controls.Add(lblGame);
            
            txtGamePath = new TextBox { Location = new Point(20, y + 20), Width = 450 };
            txtGamePath.Text = @"C:\Program Files (x86)\Steam\steamapps\common\ELDEN RING\Game";
            this.Controls.Add(txtGamePath);
            
            Button btnGame = new Button { Text = "Browse", Location = new Point(480, y + 18), Width = 80 };
            btnGame.Click += (s, e) => txtGamePath.Text = SelectFolder(txtGamePath.Text);
            this.Controls.Add(btnGame);

            y += 60;

            Label lblMod = new Label { Text = "ModEngine folder (where 'parts' folder is located):", Location = new Point(20, y), Width = 300 };
            this.Controls.Add(lblMod);
            
            txtModPath = new TextBox { Location = new Point(20, y + 20), Width = 450 };
            this.Controls.Add(txtModPath);
            
            Button btnMod = new Button { Text = "Browse", Location = new Point(480, y + 18), Width = 80 };
            btnMod.Click += (s, e) => txtModPath.Text = SelectFolder();
            this.Controls.Add(btnMod);

            y += 70;

            cbHead = CreateArmorDropdown("Head (hd):", "hd", y); y += 40;
            cbBody = CreateArmorDropdown("Body (bd):", "bd", y); y += 40;
            cbArms = CreateArmorDropdown("Arms (am):", "am", y); y += 40;
            cbLegs = CreateArmorDropdown("Legs (lg):", "lg", y); y += 50;

            Button btnPatch = new Button { Text = "Create Mod", Location = new Point(130, y), Width = 200, Height = 40, BackColor = Color.LightGreen };
            btnPatch.Click += BtnPatch_Click;
            this.Controls.Add(btnPatch);

            y += 50;
            Label lblEdit = new Label { Text = "Manual Mask Editor Slot:", Location = new Point(20, y + 4), Width = 140 };
            this.Controls.Add(lblEdit);

            cbEditSlot = new ComboBox { Location = new Point(160, y), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cbEditSlot.Items.AddRange(new object[] { "Head (10000)", "Body (10100)", "Arms (10200)", "Legs (10300)" });
            cbEditSlot.SelectedIndex = 1;
            this.Controls.Add(cbEditSlot);

            Button btnManualMask = new Button { Text = "Edit Masks Manually", Location = new Point(320, y - 2), Width = 240, Height = 30 };
            btnManualMask.Click += BtnManualMask_Click;
            this.Controls.Add(btnManualMask);
        }

        private ComboBox CreateArmorDropdown(string labelText, string partPrefix, int yPos)
        {
            Label lbl = new Label { Text = labelText, Location = new Point(20, yPos + 4), Width = 100 };
            this.Controls.Add(lbl);

            ComboBox cb = new ComboBox { Location = new Point(130, yPos), Width = 430, DropDownStyle = ComboBoxStyle.DropDownList };
            
            var filtered = armorList
                .Where(a => a.Part == partPrefix || a.Part == "all")
                .OrderBy(a => a.ID == 0 ? 0 : 1) 
                .ThenBy(a => a.SortName)         
                .ToList();
                
            cb.DataSource = filtered;
            cb.DisplayMember = "Name";
            cb.ValueMember = "ID";
            
            this.Controls.Add(cb);
            return cb;
        }

        private string SelectFolder(string startPath = "")
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (Directory.Exists(startPath)) fbd.SelectedPath = startPath;
                if (fbd.ShowDialog() == DialogResult.OK) return fbd.SelectedPath;
            }
            return startPath;
        }

        private PARAM.Row FindProtectorRowDynamic(PARAM protectorParam, int idValue, int expectedCategory)
        {
            if (idValue == 0 || protectorParam == null) return null;
            
            var match = protectorParam.Rows.Find(r =>
            {
                var modelCell = r["equipModelId"];
                var catCell = r["protectorCategory"];
                
                bool isModelMatch = modelCell != null && int.TryParse(modelCell.Value?.ToString(), out int mId) && mId == idValue;
                bool isCatMatch = catCell != null && int.TryParse(catCell.Value?.ToString(), out int cId) && cId == expectedCategory;
                
                return isModelMatch && isCatMatch;
            });
            
            if (match != null) return match;

            return protectorParam.Rows.Find(r => (int)r.ID == idValue);
        }

        private void BtnPatch_Click(object sender, EventArgs e)
        {
            string gameFolder = txtGamePath.Text.Trim();
            string modFolder = txtModPath.Text.Trim();

            if (Path.GetFileName(modFolder).Equals("parts", StringComparison.OrdinalIgnoreCase))
            {
                modFolder = Directory.GetParent(modFolder).FullName;
            }

            int headId = (int)cbHead.SelectedValue;
            int bodyId = (int)cbBody.SelectedValue;
            int armsId = (int)cbArms.SelectedValue;
            int legsId = (int)cbLegs.SelectedValue;

            string exeDir = AppDomain.CurrentDomain.BaseDirectory;
            string paramdexXmlPath = Path.Combine(exeDir, "EquipParamProtector.xml");

            if (!File.Exists(paramdexXmlPath))
            {
                MessageBox.Show("EquipParamProtector.xml not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(gameFolder) || !File.Exists(Path.Combine(gameFolder, "regulation.bin")))
            {
                MessageBox.Show("Invalid Game folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(modFolder) || !Directory.Exists(Path.Combine(modFolder, "parts")))
            {
                MessageBox.Show("Vybraná složka musí obsahovat podsložku 'parts' (nebo vyberte přímo ji)!", "Chyba složky", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string sourceReg = Path.Combine(gameFolder, "regulation.bin");
                string targetReg = Path.Combine(modFolder, "regulation.bin");
                
                File.Copy(sourceReg, targetReg, true);

                ProcessModels(modFolder, headId, bodyId, armsId, legsId);
                PatchRegulationBin(modFolder, paramdexXmlPath, headId, bodyId, armsId, legsId);
                
                MessageBox.Show("Mod has been successfully created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnManualMask_Click(object sender, EventArgs e)
        {
            string modFolder = txtModPath.Text.Trim();

            if (Path.GetFileName(modFolder).Equals("parts", StringComparison.OrdinalIgnoreCase))
            {
                modFolder = Directory.GetParent(modFolder).FullName;
            }

            string targetReg = Path.Combine(modFolder, "regulation.bin");
            if (!File.Exists(targetReg))
            {
                targetReg = Path.Combine(txtGamePath.Text.Trim(), "regulation.bin");
            }

            if (!File.Exists(targetReg))
            {
                MessageBox.Show("No regulation.bin found in mod or game folder!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int nakedId = cbEditSlot.SelectedIndex switch
            {
                0 => 10000,
                1 => 10100,
                2 => 10200,
                3 => 10300,
                _ => 10100
            };

            ComboBox selectedArmorCb = cbEditSlot.SelectedIndex switch
            {
                0 => cbHead,
                1 => cbBody,
                2 => cbArms,
                3 => cbLegs,
                _ => cbBody
            };

            int expectedCategory = cbEditSlot.SelectedIndex; // 0=Head, 1=Body, 2=Arms, 3=Legs

            int selectedVal = 0;
            if (selectedArmorCb.SelectedValue != null)
            {
                int.TryParse(selectedArmorCb.SelectedValue.ToString(), out selectedVal);
            }

            try
            {
                BND4 bnd = null;
                bool wasEncrypted = false;
                try
                {
                    bnd = BND4.Read(targetReg);
                }
                catch
                {
                    bnd = SFUtil.DecryptERRegulation(targetReg);
                    wasEncrypted = true;
                }

                var protectorFile = bnd.Files.Find(f => f.Name.EndsWith("EquipParamProtector.param", StringComparison.OrdinalIgnoreCase) || f.Name.Contains("EquipParamProtector"));
                if (protectorFile == null) throw new Exception("EquipParamProtector not found.");

                PARAM protectorParam = PARAM.Read(protectorFile.Bytes);
                string paramdexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EquipParamProtector.xml");
                if (File.Exists(paramdexPath))
                {
                    PARAMDEF paramdef = PARAMDEF.XmlDeserialize(paramdexPath);
                    protectorParam.ApplyParamdef(paramdef);
                }

                var targetRow = protectorParam.Rows.Find(r => r.ID == nakedId);
                if (targetRow == null)
                {
                    MessageBox.Show($"Row ID {nakedId} not found in EquipParamProtector.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var sourceRow = FindProtectorRowDynamic(protectorParam, selectedVal, expectedCategory);

                using (var editor = new MaskEditorForm(targetRow, sourceRow))
                {
                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        protectorFile.Bytes = protectorParam.Write();
                        if (wasEncrypted)
                        {
                            SFUtil.EncryptERRegulation(targetReg, bnd);
                        }
                        else
                        {
                            bnd.Write(targetReg);
                        }
                        MessageBox.Show("Masks manually updated and saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening mask editor:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessModels(string modFolder, int headId, int bodyId, int armsId, int legsId)
        {
            string partsDir = Path.Combine(modFolder, "parts");
            string[] prefixes = { "hd", "bd", "am", "lg" };
            int[] partIds = { headId, bodyId, armsId, legsId };
            string[] suffixes = { "_f_0000", "_f_0000_l", "_m_0000", "_m_0000_l" }; 

            for (int i = 0; i < 4; i++)
            {
                if (partIds[i] == 0) continue;
                
                string prefix = prefixes[i];
                
                var existingFiles = Directory.GetFiles(partsDir, $"{prefix}*.dcx").ToList();
                
                if (existingFiles.Count == 0) continue;

                var sourceFile = existingFiles.FirstOrDefault(f => !f.Contains("0000")) ?? existingFiles.First();

                var targetFiles = suffixes.Select(s => Path.Combine(partsDir, $"{prefix}{s}.partsbnd.dcx")).ToList();

                foreach (var targetFile in targetFiles)
                {
                    if (!sourceFile.Equals(targetFile, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(sourceFile, targetFile, true);
                    }
                }

                foreach (var file in existingFiles)
                {
                    bool isTarget = targetFiles.Any(t => t.Equals(file, StringComparison.OrdinalIgnoreCase));
                    if (!isTarget)
                    {
                        File.Delete(file);
                    }
                }
            }
        }

        private void PatchRegulationBin(string baseFolder, string paramdexPath, int headId, int bodyId, int armsId, int legsId)
        {
            string regPath = Path.Combine(baseFolder, "regulation.bin");
            if (!File.Exists(regPath)) throw new FileNotFoundException($"regulation.bin not found in {regPath}");

            BND4 bnd = null;
            bool wasEncrypted = false;

            try
            {
                bnd = BND4.Read(regPath);
            }
            catch
            {
                bnd = SFUtil.DecryptERRegulation(regPath);
                wasEncrypted = true;
            }

            var protectorFile = bnd.Files.Find(f => f.Name.EndsWith("EquipParamProtector.param", StringComparison.OrdinalIgnoreCase) || f.Name.Contains("EquipParamProtector"));
            if (protectorFile == null) throw new Exception("EquipParamProtector not found in regulation.bin BND4.");

            PARAM protectorParam = PARAM.Read(protectorFile.Bytes);

            if (File.Exists(paramdexPath))
            {
                PARAMDEF paramdef = PARAMDEF.XmlDeserialize(paramdexPath);
                protectorParam.ApplyParamdef(paramdef);
            }

            var idMap = new (int NakedId, int SelectedVal, int Category)[]
            {
                (10000, headId, 0),
                (10100, bodyId, 1),
                (10200, armsId, 2),
                (10300, legsId, 3)
            };

            bool modified = false;

            foreach (var pair in idMap)
            {
                if (pair.SelectedVal == 0) continue;

                var sourceRow = FindProtectorRowDynamic(protectorParam, pair.SelectedVal, pair.Category);
                if (sourceRow == null) continue;

                var nakedRow = protectorParam.Rows.Find(r => r.ID == pair.NakedId);
                if (nakedRow != null)
                {
                    foreach (var cell in sourceRow.Cells)
                    {
                        if (cell.Def != null)
                        {
                            string cName = cell.Def.InternalName;

                            if (cName.Contains("invisibleFlag_SexVer", StringComparison.OrdinalIgnoreCase))
                            {
                                var targetCell = nakedRow[cName];
                                if (targetCell != null) targetCell.Value = cell.Value;
                            }
                        }
                    }
                    modified = true;
                }
            }

            if (modified)
            {
                protectorFile.Bytes = protectorParam.Write();
                
                if (wasEncrypted)
                {
                    SFUtil.EncryptERRegulation(regPath, bnd);
                }
                else
                {
                    bnd.Write(regPath);
                }
            }
        }
    }       

    public class ArmorItem
    {
        public string Part { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string SortName { get; set; }
    }

    public class MaskEditorForm : Form
    {
        private PARAM.Row targetRow;
        private PARAM.Row sourceRow;
        private DataGridView dgvMasks;
        private Button btnSave;

        private static readonly Dictionary<int, string> KnownMaskNames = new Dictionary<int, string>
        {
            { 0, "Upper face" }, { 1, "Chin" }, { 2, "Nose and cheeks" }, { 3, "Ears and top of head" },
            { 4, "Neck" }, { 5, "Lower neck" }, { 6, "Chest" }, { 7, "Upper elbows" },
            { 8, "Shoulders" }, { 9, "Lower elbows" }, { 10, "Lower arms" }, { 11, "Right hand" },
            { 12, "Waist" }, { 13, "Left hand" }, { 14, "Knees" }, { 15, "Lower legs" },
            { 16, "Feet" }, { 17, "Eyepatch" }, { 20, "[ARMS] Long Gloves (Lower Arm)" }, { 21, "[ARMS] Long Gloves (Full Length)" },
            { 23, "[BODY] Scarf / High Collar (Compressed)" }, { 24, "[BODY] Sleeves (Bunched to Elbow)" }, { 25, "[BODY] Sleeves (Bunched Midway)" },
            { 26, "[BODY] Sleeves (Full)" }, { 27, "[BODY] Large Sleeves (Bunched to Elbow)" }, { 28, "[BODY] Large Sleeves (Full)" },
            { 29, "[BODY] Sleeves (Wrist Bracelets)" }, { 30, "[BODY] Couter / Elbow Armor" }, { 31, "[BODY] Couter / Elbow Armor (Higher)" },
            { 32, "[BODY] Small Collar" }, { 33, "[BODY] Small Collar (Compressed)" }, { 34, "[BODY] Scarf / High Collar (Full)" },
            { 35, "[BODY] Lower Abdomen Cover" }, { 36, "[BODY] Small Hood (Down)" }, { 37, "[BODY] Small Hood (Up)" },
            { 38, "[BODY] Left Pauldron / Shoulder" }, { 39, "[BODY] Right Pauldron / Shoulder" }, { 40, "[BODY] Cowl" },
            { 41, "[BODY] Cowl (Long)" }, { 42, "[BODY] Cowl (Mid)" }, { 43, "[BODY] Cowl (Compressed)" },
            { 44, "[HEAD] Gorget / Neckpiece" }, { 45, "[HEAD] Gorget / Neckpiece (Compressed)" }, { 46, "[HEAD] Long Hood / Plume (Low)" },
            { 47, "[HEAD] Long Hood / Plume (Mid)" }, { 48, "[HEAD] Long Hood / Plume (High)" }, { 49, "[HEAD] Long Hood / Plume (Short)" },
            { 50, "[LEGS] High Waistbelt" }, { 51, "[LEGS] Leggings" }, { 52, "[LEGS] Leggings (Compressed)" },
            { 53, "[LEGS] Kneepads" }, { 54, "[LEGS] Kneepads (Compressed)" }, { 55, "[LEGS] Waistbelt" },
            { 56, "[LEGS] Waistbelt (Compressed)" }, { 57, "[LEGS] Waistcloth" }, { 58, "[LEGS] Waistcloth (Compressed)" },
            { 59, "[LEGS] Pants Big Thighs" }, { 60, "Hair (Front)" }, { 61, "Hair (Forehead)" },
            { 62, "Hair (Under Helmet)" }, { 63, "Hair (Over Headband)" }, { 64, "Hair (Full)" },
            { 65, "Hair (Back of Head)" }, { 66, "Long Hair Braid / Tail (Low)" }, { 67, "Long Hair Braid / Tail (High)" },
            { 68, "Long Hair Braid / Tail (Highest)" }, { 69, "Long Hair Braid / Tail (Lowest, Short)" },
            { 70, "[HEAD] Gorget / Neckpiece (Large)" }, { 71, "[BODY] Long Shirt (Bunched up to Belt)" }, { 72, "[BODY] Long Shirt (Full Length over Belt)" },
            { 73, "Bare Torso and Upper Arms" }, { 74, "[BODY] Gravekeeper Cloak Hood (Down)" }, { 75, "[BODY] Gravekeeper Cloak Hood (Up)" },
            { 76, "Lower Neck Warp" }, { 78, "Beard Jaw" }, { 79, "Beard Chin" }, { 80, "Beard Stubble" }
        };

        public MaskEditorForm(PARAM.Row target, PARAM.Row source)
        {
            targetRow = target;
            sourceRow = source;
            Text = $"Manual Mask Editor - Target ID: {target.ID} | Source Row: {(source?.ID.ToString() ?? "None / Skip")}";
            Size = new Size(680, 650);
            StartPosition = FormStartPosition.CenterParent;

            dgvMasks = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvMasks.Columns.Add("ColName", "Mask / Field Name");
            dgvMasks.Columns["ColName"].ReadOnly = true;

            var chkCol = new DataGridViewCheckBoxColumn { Name = "ColVal", HeaderText = "Target Hidden (Edit)" };
            dgvMasks.Columns.Add(chkCol);

            var srcCol = new DataGridViewTextBoxColumn { Name = "ColSourceVal", HeaderText = "Source Armor Val", ReadOnly = true };
            dgvMasks.Columns.Add(srcCol);

            btnSave = new Button { Text = "Save Changes", Dock = DockStyle.Bottom, Height = 45 };
            btnSave.Click += BtnSave_Click;

            Controls.Add(dgvMasks);
            Controls.Add(btnSave);

            LoadMasks();
        }

        private void LoadMasks()
        {
            dgvMasks.Rows.Clear();
            foreach (var cell in targetRow.Cells)
            {
                if (cell.Def != null && cell.Def.InternalName.Contains("invisibleFlag_SexVer", StringComparison.OrdinalIgnoreCase))
                {
                    string name = cell.Def.InternalName;
                    long val = Convert.ToInt64(cell.Value);

                    string srcValStr = "No source selected";
                    if (sourceRow != null)
                    {
                        var srcCell = sourceRow.Cells.FirstOrDefault(c => c.Def != null && c.Def.InternalName.Equals(name, StringComparison.OrdinalIgnoreCase));
                        if (srcCell != null)
                        {
                            srcValStr = srcCell.Value?.ToString() ?? "0";
                        }
                        else
                        {
                            srcValStr = "Cell missing";
                        }
                    }

                    var match = Regex.Match(name, @"\d+");
                    string label = name;
                    if (match.Success && int.TryParse(match.Value, out int idxNum))
                    {
                        if (KnownMaskNames.TryGetValue(idxNum, out string knownDesc))
                        {
                            label = $"Mask {idxNum:00} - {knownDesc}";
                        }
                        else
                        {
                            label = $"Mask {idxNum:00} - Unknown ({name})";
                        }
                    }

                    int idx = dgvMasks.Rows.Add(label, val != 0, srcValStr);
                    dgvMasks.Rows[idx].Tag = cell;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvMasks.Rows)
            {
                if (row.Tag is PARAM.Cell cell)
                {
                    bool isChecked = Convert.ToBoolean(row.Cells["ColVal"].Value);
                    cell.Value = isChecked ? (byte)1 : (byte)0;
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}