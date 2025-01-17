﻿using CASCLib;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using wow.tools.local.Services;

namespace wow.tools.local.Controllers
{
    [Route("keys/")]
    [ApiController]
    public class KeyController : Controller
    {
        private readonly DBCManager dbcManager;
        private readonly Dictionary<ulong, (int ID, string FirstSeen, string Description)> KeyInfo = new();

        public KeyController(IDBCManager dbcManager)
        {
            this.dbcManager = dbcManager as DBCManager;

            KeyInfo.Add(0xFA505078126ACB3E, (15, "WOW-20740patch7.0.1_Beta", "not used between 7.0 and 7.3"));
            KeyInfo.Add(0xFF813F7D062AC0BC, (25, "WOW-20740patch7.0.1_Beta", "not used between 7.0 and 7.3"));
            KeyInfo.Add(0xD1E9B5EDF9283668, (39, "WOW-20740patch7.0.1_Beta", "Enchanted Torch pet"));
            KeyInfo.Add(0xB76729641141CB34, (40, "WOW-20740patch7.0.1_Beta", "Enchanted Pen pet"));
            KeyInfo.Add(0xFFB9469FF16E6BF8, (41, "WOW-20740patch7.0.1_Beta", "not used between 7.0 and 7.3"));
            KeyInfo.Add(0x23C5B5DF837A226C, (42, "WOW-20740patch7.0.1_Beta", "Enchanted Cauldron pet"));
            KeyInfo.Add(0x3AE403EF40AC3037, (51, "WOW-21249patch7.0.3_Beta", "not used between 7.0 and 7.3"));
            KeyInfo.Add(0xE2854509C471C554, (52, "WOW-21249patch7.0.3_Beta", "Warcraft movie items"));
            KeyInfo.Add(0x8EE2CB82178C995A, (55, "WOW-21531patch7.0.3_Beta", "BlizzCon 2016 Murlocs"));
            KeyInfo.Add(0x5813810F4EC9B005, (56, "WOW-21531patch7.0.3_Beta", "Fel Kitten"));
            KeyInfo.Add(0x7F9E217166ED43EA, (57, "WOW-21531patch7.0.3_Beta", "Legion music "));
            KeyInfo.Add(0xC4A8D364D23793F7, (58, "WOW-21691patch7.0.3_Beta", "Demon Hunter #1 cinematic (legion_dh1)"));
            KeyInfo.Add(0x40A234AEBCF2C6E5, (59, "WOW-21691patch7.0.3_Beta", "Demon Hunter #2 cinematic (legion_dh2)"));
            KeyInfo.Add(0x9CF7DFCFCBCE4AE5, (60, "WOW-21691patch7.0.3_Beta", "Val'sharah #1 cinematic (legion_val_yd)"));
            KeyInfo.Add(0x4E4BDECAB8485B4F, (61, "WOW-21691patch7.0.3_Beta", "Val'sharah #2 cinematic (legion_val_yx)"));
            KeyInfo.Add(0x94A50AC54EFF70E4, (62, "WOW-21691patch7.0.3_Beta", "Sylvanas warchief cinematic (legion_org_vs)"));
            KeyInfo.Add(0xBA973B0E01DE1C2C, (63, "WOW-21691patch7.0.3_Beta", "Stormheim Sylvanas vs Greymane cinematic (legion_sth)"));
            KeyInfo.Add(0x494A6F8E8E108BEF, (64, "WOW-21691patch7.0.3_Beta", "Harbingers Gul'dan video (legion_hrb_g)"));
            KeyInfo.Add(0x918D6DD0C3849002, (65, "WOW-21691patch7.0.3_Beta", "Harbingers Khadgar video (legion_hrb_k)"));
            KeyInfo.Add(0x0B5F6957915ADDCA, (66, "WOW-21691patch7.0.3_Beta", "Harbingers Illidan video (legion_hrb_i)"));
            KeyInfo.Add(0x794F25C6CD8AB62B, (67, "WOW-21846patch7.0.3_Beta", "Suramar cinematic (legion_su_i)"));
            KeyInfo.Add(0xA9633A54C1673D21, (68, "WOW-21846patch7.0.3_Beta", "legion_su_r cinematic"));
            KeyInfo.Add(0x5E5D896B3E163DEA, (69, "WOW-21846patch7.0.3_Beta", "Broken Shore intro cinematic (legion_bs_i)"));
            KeyInfo.Add(0x0EBE36B5010DFD7F, (70, "WOW-21846patch7.0.3_Beta", "Alliance Broken Shore cinematic (legion_bs_a)"));
            KeyInfo.Add(0x01E828CFFA450C0F, (71, "WOW-21846patch7.0.3_Beta", "Horde Broken Shore cinematic (legion_bs_h)"));
            KeyInfo.Add(0x4A7BD170FE18E6AE, (72, "WOW-21846patch7.0.3_Beta", "Khadgar & Light's Heart cinematic (legion_iq_lv)"));
            KeyInfo.Add(0x69549CB975E87C4F, (73, "WOW-21846patch7.0.3_Beta", "legion_iq_id cinematic"));
            KeyInfo.Add(0x460C92C372B2A166, (74, "WOW-21952patch7.0.3_Beta", "Stormheim Alliance cinematic (legion_g_a_sth)"));
            KeyInfo.Add(0x8165D801CCA11962, (75, "WOW-21952patch7.0.3_Beta", "Stormheim Horde cinematic (legion_g_h_sth)"));
            KeyInfo.Add(0xA3F1C999090ADAC9, (81, "WOW-22578patch7.1.0_PTR", "Firecat Mount"));
            KeyInfo.Add(0x18AFDF5191923610, (82, "WOW-22578patch7.1.0_PTR", "not used between 7.1 and 7.3"));
            KeyInfo.Add(0x3C258426058FBD93, (91, "WOW-23436patch7.2.0_PTR", "not used between 7.2 and 7.3"));
            KeyInfo.Add(0x094E9A0474876B98, (92, "WOW-23910patch7.2.5_PTR", "shadowstalkerpanthermount, shadowstalkerpantherpet"));
            KeyInfo.Add(0x3DB25CB86A40335E, (93, "WOW-23789patch7.2.0_PTR", "legion_72_ots"));
            KeyInfo.Add(0x0DCD81945F4B4686, (94, "WOW-23789patch7.2.0_PTR", "legion_72_tst"));
            KeyInfo.Add(0x486A2A3A2803BE89, (95, "WOW-23789patch7.2.0_PTR", "legion_72_ars"));
            KeyInfo.Add(0x71F69446AD848E06, (97, "WOW-24473patch7.3.0_PTR", "BlizzCon 2017 Mounts (AllianceShipMount and HordeZeppelinMount)"));
            KeyInfo.Add(0x211FCD1265A928E9, (98, "WOW-24473patch7.3.0_PTR", "Shadow fox pet (store) "));
            KeyInfo.Add(0x0ADC9E327E42E98C, (99, "WOW-23910patch7.2.5_PTR", "legion_72_tsf"));
            KeyInfo.Add(0xBAE9F621B60174F1, (100, "WOW-24727patch7.3.0_PTR", "Rejection of the Gift cinematic (legion_73_agi)"));
            KeyInfo.Add(0x34DE1EEADC97115E, (101, "WOW-24727patch7.3.0_PTR", "Resurrection of Alleria Windrunner cinematic (legion_73_avt)"));
            KeyInfo.Add(0xE07E107F1390A3DF, (102, "WOW-25079patch7.3.2_PTR", "Tottle battle pet, Raptor mount, Horse mount (104 files)"));
            KeyInfo.Add(0x32690BF74DE12530, (103, "WOW-24781patch7.3.0_PTR", "legion_73_pan"));
            KeyInfo.Add(0xBF3734B1DCB04696, (104, "WOW-25079patch7.3.2_PTR", "legion_73_afn"));
            KeyInfo.Add(0x74F4F78002A5A1BE, (105, "WOW-25079patch7.3.2_PTR", "SilithusPhase01 map"));
            KeyInfo.Add(0x423F07656CA27D23, (107, "WOW-25600patch7.3.5_PTR", "bltestmap"));
            KeyInfo.Add(0x0691678F83E8A75D, (108, "WOW-25600patch7.3.5_PTR", "filedataid 1782602-1782603"));
            KeyInfo.Add(0x324498590F550556, (109, "WOW-25600patch7.3.5_PTR", "filedataid 1782615-1782619"));
            KeyInfo.Add(0xC02C78F40BEF5998, (110, "WOW-25600patch7.3.5_PTR", "test/testtexture.blp (fdid 1782613)"));
            KeyInfo.Add(0x47011412CCAAB541, (111, "WOW-25600patch7.3.5_PTR", "unused in 25600"));
            KeyInfo.Add(0x23B6F5764CE2DDD6, (112, "WOW-25600patch7.3.5_PTR", "unused in 25600"));
            KeyInfo.Add(0x8E00C6F405873583, (113, "WOW-25600patch7.3.5_PTR", "tileset/test/bltesttileset*.blp"));
            KeyInfo.Add(0x78482170E4CFD4A6, (114, "WOW-25600patch7.3.5_PTR", "Magni Bronzebeard VO"));
            KeyInfo.Add(0xB1EB52A64BFAF7BF, (115, "WOW-25600patch7.3.5_PTR", "dogmount, 50 files"));
            KeyInfo.Add(0xFC6F20EE98D208F6, (117, "WOW-25632patch7.3.5_PTR", "bfa shop stuff"));
            KeyInfo.Add(0x402CFABF2020D9B7, (118, "WOW-25678patch7.3.5_PTR", "bfa ad texture"));
            KeyInfo.Add(0x6FA0420E902B4FBE, (119, "WOW-25744patch7.3.5_PTR", "Legion epilogue cinematics"));
            KeyInfo.Add(0x1076074F2B350A2D, (121, "WOW-26287patch8.0.1_Beta", "skiff"));
            KeyInfo.Add(0x816F00C1322CDF52, (122, "WOW-26287patch8.0.1_Beta", "snowkid"));
            KeyInfo.Add(0xDDD295C82E60DB3C, (123, "WOW-26287patch8.0.1_Beta", "redbird"));
            KeyInfo.Add(0x83E96F07F259F799, (124, "WOW-26522patch8.0.1_Beta", "BlizzCon 2018 (Alliance and Horde banners and cloaks)"));
            KeyInfo.Add(0x49FBFE8A717F03D5, (225, "WOW-27826patch8.1.0_PTR", "Meatwagon mount (Warcraft 3: Reforged)"));
            KeyInfo.Add(0xC1E5D7408A7D4484, (226, "WOW-26871patch8.0.1_Beta", "Sylvanas Warbringer cinematic"));
            KeyInfo.Add(0xE46276EB9E1A9854, (227, "WOW-26871patch8.0.1_Beta", "ltc_a, ltc_h and ltt cinematics"));
            KeyInfo.Add(0xD245B671DD78648C, (228, "WOW-26871patch8.0.1_Beta", "stz, zia, kta, jnm & ja cinematics"));
            KeyInfo.Add(0x4C596E12D36DDFC3, (229, "WOW-26871patch8.0.1_Beta", "bar cinematic"));
            KeyInfo.Add(0x0C9ABD5081C06411, (230, "WOW-26871patch8.0.1_Beta", "zcf cinematic"));
            KeyInfo.Add(0x3C6243057F3D9B24, (231, "WOW-26871patch8.0.1_Beta", "ktf cinematic"));
            KeyInfo.Add(0x7827FBE24427E27D, (232, "WOW-26871patch8.0.1_Beta", "rot cinematic"));
            KeyInfo.Add(0xFAF9237E1186CF66, (233, "WOW-28048patch8.1.0_PTR", "DB2 partial encryption test battle pet"));
            KeyInfo.Add(0x5DD92EE32BBF9ABD, (234, "WOW-27004patch8.0.1_Subm", "interface/icons/ui_shop_bcv.blp"));
            KeyInfo.Add(0x0B68A7AF5F85F7EE, (236, "WOW-28151patch8.1.0_PTR", "flying pig mount"));
            KeyInfo.Add(0x01531713C83FCC39, (237, "WOW-28151patch8.1.0_PTR", "16th anniversary cape"));
            KeyInfo.Add(0x76E4F6739A35E8D7, (238, "WOW-28294patch8.1.0_PTR", "Sylverian Dreamer mount"));
            KeyInfo.Add(0x66033F28DC01923C, (239, "WOW-28294patch8.1.0_PTR", "Vulpine Familiar mount"));
            KeyInfo.Add(0xFCF34A9B05AE7E6A, (240, "WOW-28151patch8.1.0_PTR", "Alliance fireworks"));
            KeyInfo.Add(0xE2F6BD41298A2AB9, (241, "WOW-28151patch8.1.0_PTR", "Horde fireworks"));
            KeyInfo.Add(0x14C4257E557B49A1, (242, "WOW-28440patch8.1.0_PTR", "dor cinematic"));
            KeyInfo.Add(0x1254E65319C6EEFF, (243, "WOW-28440patch8.1.0_PTR", "akt cinematic"));
            KeyInfo.Add(0xC8753773ADF1174C, (244, "WOW-28938patch8.1.5_PTR", "Obsidian Worldbreaker mount & Lil' Nefarian pet"));
            KeyInfo.Add(0x2170BCAA9FA96E22, (245, "WOW-28938patch8.1.5_PTR", "baby alpaca pet"));
            KeyInfo.Add(0x75485627AA225F4D, (246, "WOW-28938patch8.1.5_PTR", "fdid 2741546 (a creature that uses baby raptor pet sounds), 2741548, 2741549"));
            KeyInfo.Add(0x08717B15BF3C7955, (248, "WOW-29220patch8.1.5_PTR", "inv_encrypted20.blp (fdid 2823166)"));
            KeyInfo.Add(0xD19DCF7ACA8D96D6, (249, "WOW-30080patch8.2.0_PTR", "Steamscale Incinerator mount"));
            KeyInfo.Add(0x9FD609902B4B2E07, (250, "WOW-29418patch8.1.5_PTR", "Derek Proudmoore cinematic (dpr, 5 files)"));
            KeyInfo.Add(0xCB26B441FAE4C8CD, (251, "WOW-30080patch8.2.0_PTR", "Mechanical Dragonling pet"));
            KeyInfo.Add(0xA98C7594F55C02F0, (252, "WOW-30080patch8.2.0_PTR", "BlizzCon 2019 - Murloc pets"));
            KeyInfo.Add(0x259EE68CD9E76DBA, (253, "WOW-30080patch8.2.0_PTR", "Alabaster mounts (30 files)"));
            KeyInfo.Add(0x6A026290FBDB3754, (255, "WOW-30080patch8.2.0_PTR", "BlizzCon 2019 - Wendigo transmog set"));
            KeyInfo.Add(0xCF72FD04608D36ED, (257, "WOW-30262patch8.2.0_PTR", "Azshara Warbringer cinematic (5 files)"));
            KeyInfo.Add(0x17F07C2E3A45DB3D, (258, "WOW-30262patch8.2.0_PTR", "Solesa Naksu Nazjatar phase (34 files)"));
            KeyInfo.Add(0xDFAB5841B87802B5, (259, "WOW-31337patch8.2.5_PTR", "Rat mount"));
            KeyInfo.Add(0xC050FA06BB0538F6, (260, "WOW-30495patch8.2.0_PTR", "Crossroads cinematic (5 files)"));
            KeyInfo.Add(0xAB5CDD3FC321831F, (261, "WOW-30495patch8.2.0_PTR", "Azshara kill cinematic (5 files)"));
            KeyInfo.Add(0xA7B7D1F12395040E, (262, "WOW-30495patch8.2.0_PTR", "Nazjatar intro cinematics (9 files)"));
            KeyInfo.Add(0x83A2AB72DD8AE992, (263, "WOW-31337patch8.2.5_PTR", "8.2.5 War Campaign scenario/models"));
            KeyInfo.Add(0xBEAF567CC45362F0, (264, "WOW-31337patch8.2.5_PTR", "8.2.5 War Campaign quests/vo"));
            KeyInfo.Add(0x7BB3A77FD8D14783, (265, "WOW-31337patch8.2.5_PTR", "8.2.5 War Campaign epilogue quests"));
            KeyInfo.Add(0x8F4098E2470FE0C8, (266, "WOW-31337patch8.2.5_PTR", "8.2.5 War Campaign epilogue in-game cinematic"));
            KeyInfo.Add(0x6AC5C837A2027A6B, (267, "WOW-31337patch8.2.5_PTR", "Shadowlands CE rewards"));
            KeyInfo.Add(0x302AAD8B1F441D95, (271, "WOW-31337patch8.2.5_PTR", "RaF mounts & armor"));
            KeyInfo.Add(0x5C909F00088734B9, (272, "WOW-31337patch8.2.5_PTR", "Creature with Yak sounds"));
            KeyInfo.Add(0xF785977C76DE9C77, (273, "WOW-31337patch8.2.5_PTR", "Faerie Dragon transmog set"));
            KeyInfo.Add(0x1CDAF3931871BEC3, (275, "WOW-31337patch8.2.5_PTR", "Winter Veil 2019"));
            KeyInfo.Add(0x814E1AB43F3F9345, (276, "WOW-31599patch8.2.5_PTR", "The Negotiation cinematic (5 files)"));
            KeyInfo.Add(0x1FBE97A317FFBEFA, (277, "WOW-31599patch8.2.5_PTR", "Reckoning cinematic (5 files)"));
            KeyInfo.Add(0x30581F81528FB27C, (278, "WOW-32044patch8.3.0_PTR", "Contains creature that uses monkey sounds"));
            KeyInfo.Add(0x4287F49A5BB366DA, (279, "WOW-31599patch8.2.5_PTR", "Unused in 8.2.5"));
            KeyInfo.Add(0xD134F430A45C1CF2, (280, "WOW-32044patch8.3.0_PTR", "8.3 Deathknight Scenario"));
            KeyInfo.Add(0x01C82EE0725EDA3A, (281, "WOW-31812patch8.2.5_PTR", "Unused in 8.2.5"));
            KeyInfo.Add(0x04C0C50B5BE0CC78, (282, "WOW-31812patch8.2.5_PTR", "Unused in 8.2.5"));
            KeyInfo.Add(0xA26FD104489B3DE5, (283, "WOW-31812patch8.2.5_PTR", "Unused in 8.2.5"));
            KeyInfo.Add(0xEA6C3B8F210A077F, (284, "WOW-32044patch8.3.0_PTR", "Gargantuan Grrloc mount"));
            KeyInfo.Add(0x4A738212694AD0B6, (285, "WOW-32044patch8.3.0_PTR", "Sunwarmed Furline mount"));
            KeyInfo.Add(0x2A430C60DDCC75FF, (286, "WOW-32044patch8.3.0_PTR", "Item (set?)"));
            KeyInfo.Add(0x0A096FB251CFF471, (287, "WOW-32414patch8.3.0_PTR", "N'Zoth scenescript/VO"));
            KeyInfo.Add(0x205AFFCDFBA639CB, (288, "WOW-32414patch8.3.0_PTR", "Unused as of 8.3.0.32414"));
            KeyInfo.Add(0x32B62CF10571971F, (289, "WOW-32489patch8.3.0_PTR", "In-game Wrathion scene"));
            KeyInfo.Add(0xB408D6CDE8E0D4C1, (290, "WOW-33978patch9.0.1_Beta", "Maldraxxus IGC"));
            KeyInfo.Add(0x1DBE03EF5A0059E1, (294, "WOW-32489patch8.3.0_PTR", "Anduin/Wrathion cinematic"));
            KeyInfo.Add(0x29D08CEA080FDB84, (295, "WOW-32489patch8.3.0_PTR", "Wrathion vs N'Zoth cinematic"));
            KeyInfo.Add(0x3FE91B3FD7F18B37, (296, "WOW-32489patch8.3.0_PTR", "N'Zoth end cinematic"));
            KeyInfo.Add(0xF7BECC6682B9EF36, (297, "WOW-33978patch9.0.1_Beta", "NPE/Death rising cinematics"));
            KeyInfo.Add(0xDCB5C5DC78520BD6, (298, "WOW-33978patch9.0.1_Beta", "Nathanos cinematic/cutscene"));
            KeyInfo.Add(0x566DF4A5A9E3341F, (299, "WOW-33978patch9.0.1_Beta", "cutscene + Enter the Maw cinematic"));
            KeyInfo.Add(0x9183F8AAA603704D, (300, "WOW-33978patch9.0.1_Beta", "Kyrian cutscene"));
            KeyInfo.Add(0x856D38B447512C51, (301, "WOW-33978patch9.0.1_Beta", "Covenant council cutscene"));
            KeyInfo.Add(0x1D0614B43A9D6DF9, (302, "WOW-33978patch9.0.1_Beta", "Bastion covenant sanctum attack"));
            KeyInfo.Add(0x19742EF8BC509417, (303, "WOW-34137patch9.0.1_Beta", "Willowblossom scene"));
            KeyInfo.Add(0x0A88670B2C572700, (304, "WOW-34137patch9.0.1_Beta", "Bastion covenant sanctum attack prelude"));
            KeyInfo.Add(0xDA2615B5C0237D39, (306, "WOW-34199patch9.0.1_Beta", "Purple Baby Murloc pet"));
            KeyInfo.Add(0xB6FF5BC63B2F8172, (307, "WOW-34490patch9.0.1_Beta", "Sunny pet"));
            KeyInfo.Add(0x90E01E041D38A8B0, (309, "WOW-34821patch9.0.1_Beta", "Ardenweald cinematic + cutscene"));
            KeyInfo.Add(0x8FD76F6044F9AAB1, (310, "WOW-34821patch9.0.1_Beta", "Venthyr cutscene"));
            KeyInfo.Add(0x40377D9CE69C6E30, (311, "WOW-34902patch9.0.1_Beta", "Maw intro ending cutscene"));
            KeyInfo.Add(0xFDEE9569100B1D53, (312, "WOW-34972patch9.0.1_Beta", "Maw intro 2nd cutscene"));
            KeyInfo.Add(0x4F68D9D5A1918F0D, (313, "WOW-35167patch9.0.1_Beta", "Maldraxxus cutscene"));
            KeyInfo.Add(0x99882D68AADCFA6D, (314, "WOW-35167patch9.0.1_Beta", "Also a Venthyr scene"));
            KeyInfo.Add(0x02CC0FC116A9C190, (315, "WOW-35167patch9.0.1_Beta", "Runecarver/Jailer scene"));
            KeyInfo.Add(0xBC5C79FC6E592D81, (316, "WOW-35167patch9.0.1_Beta", "Jailer scene"));
            KeyInfo.Add(0xC737DD0E709977BD, (317, "WOW-35167patch9.0.1_Beta", "Maw intro 1st cutscene"));
            KeyInfo.Add(0x33C93E43A1846B30, (318, "WOW-35167patch9.0.1_Beta", "Necrolord scene"));
            KeyInfo.Add(0x240745D093CEBD04, (319, "WOW-35167patch9.0.1_Beta", "fdid 801754, 801755, 801756, 1758037"));
            KeyInfo.Add(0x73E8CCF0812E8809, (320, "WOW-35256patch9.0.1_Beta", ""));
            KeyInfo.Add(0xED4224DDF3776EB0, (322, "WOW-35522patch9.0.1_PTR", "bear mount"));
            KeyInfo.Add(0x60C7EDA6A7BCDED0, (323, "WOW-38312patch9.1.0_PTR", "Celestial transmog set"));
            KeyInfo.Add(0x1297977C87A557D5, (324, "WOW-35854patch9.0.2_Beta", ""));
            KeyInfo.Add(0x6CD8165A18D613CA, (325, "WOW-35854patch9.0.2_Beta", "dragonwhelpoutland2_light battle pet"));
            KeyInfo.Add(0x3B5D811B6C4B0987, (326, "WOW-35917patch9.0.1_Retail", "bta/lc cinematic"));
            KeyInfo.Add(0x2513CE4CF5A5DACB, (327, "WOW-35917patch9.0.1_Retail", "rme cinematic"));
            KeyInfo.Add(0xCE6A8C3E23432875, (328, "WOW-35917patch9.0.1_Retail", "pim cinematic"));
            KeyInfo.Add(0x7778A0E0914354FA, (329, "WOW-35917patch9.0.1_Retail", "Sylvanas' choice cinematic"));
            KeyInfo.Add(0xC4E751C98189FA5B, (330, "WOW-35917patch9.0.1_Retail", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0xF30F2C1A6FEEF618, (331, "WOW-37503patch9.0.5_PTR", "phoenix2mount"));
            KeyInfo.Add(0x784D9A78CAB17AFC, (332, "WOW-35978patch9.0.2_Beta", ""));
            KeyInfo.Add(0xDAED22AE797E4EF1, (333, "WOW-36086patch9.0.2_Beta", ""));
            KeyInfo.Add(0x428811AD4C462334, (334, "WOW-36074patch9.0.1_PTR", "TBC:DE Warpstalker mount"));
            KeyInfo.Add(0x39044914C846DB5F, (335, "WOW-38312patch9.1.0_PTR", "Murloc backpack"));
            KeyInfo.Add(0x98D3909D3C2358A9, (338, "WOW-38312patch9.1.0_PTR", "Armored Jailer"));
            KeyInfo.Add(0x7412D6BD04C6686D, (339, "WOW-37503patch9.0.5_PTR", "Wandering Ancient mount"));
            KeyInfo.Add(0x598F3D6BC8233EA5, (340, "WOW-38312patch9.1.0_PTR", "Gurgl Murloc pet"));
            KeyInfo.Add(0x943AE6AC59D361D9, (341, "WOW-41089patch9.2.0_PTR", "Murkastrasza pet"));
            KeyInfo.Add(0xFC9637CDB85FD83E, (342, "WOW-41089patch9.2.0_PTR", "starts at fdid 921760, 44 files"));
            KeyInfo.Add(0x47FEFED7D7CE2893, (343, "WOW-38312patch9.1.0_PTR", "Soulrender/Garrosh IGC"));
            KeyInfo.Add(0xF23FDEAD4B55445F, (344, "WOW-38312patch9.1.0_PTR", "Sabellian IGC"));
            KeyInfo.Add(0xD735851256FD94F8, (345, "WOW-38312patch9.1.0_PTR", "starts at fdid 841604, 22 files"));
            KeyInfo.Add(0x0B5583337754BDB4, (346, "WOW-38312patch9.1.0_PTR", "starts at fdid 841604, 13 files"));
            KeyInfo.Add(0xC50DE5DE7388AF03, (347, "WOW-38312patch9.1.0_PTR", "Primus/Jailer IGC"));
            KeyInfo.Add(0xBC2FB09C6452F082, (348, "WOW-38312patch9.1.0_PTR", "Runecarver IGC"));
            KeyInfo.Add(0x89E1427153BD4A31, (349, "WOW-39977patch9.1.5_PTR", "Book mount"));
            KeyInfo.Add(0xBF7B1B1DB3CE52B1, (350, "WOW-38312patch9.1.0_PTR", "Uther IGC"));
            KeyInfo.Add(0xF04F377D7BD10A5E, (351, "WOW-38312patch9.1.0_PTR", "Anduin compass IGC"));
            KeyInfo.Add(0xD85B0317B72EB7DB, (352, "WOW-39977patch9.1.5_PTR", "Baby Faerie Dragon"));
            KeyInfo.Add(0x152155855D497807, (353, "WOW-39977patch9.1.5_PTR", "Mount + Pet + Item(s)"));
            KeyInfo.Add(0xB29C6246918DDF64, (354, "WOW-38627patch9.1.0_PTR", "Sage (Hearthstone rat mount)"));
            KeyInfo.Add(0xAD1D49C129D857CA, (355, "WOW-39977patch9.1.5_PTR", "Lunar New Year mount"));
            KeyInfo.Add(0x030DDEB1D4FA3FCE, (356, "WOW-38872patch9.1.0_PTR", ""));
            KeyInfo.Add(0xD874F70874B5C5FA, (357, "WOW-38872patch9.1.0_PTR", ""));
            KeyInfo.Add(0xA263FE68D73AA3DA, (358, "WOW-38950patch9.1.0_PTR", "Sanctum of Domination cinematic"));
            KeyInfo.Add(0x1E04A8DDB3C1DC0B, (359, "WOW-39015patch9.1.0_PTR", "Battle of Ardenweald cinematic"));
            KeyInfo.Add(0x512D18B506449AFC, (360, "WOW-39015patch9.1.0_PTR", "By Our Hand cinematic"));
            KeyInfo.Add(0x4903F480BD987FA9, (361, "WOW-41089patch9.2.0_PTR", "Encrypted Sylvanas variant"));
            KeyInfo.Add(0x83C9EE5007C286B8, (362, "WOW-41089patch9.2.0_PTR", "Pet / achievement"));
            KeyInfo.Add(0xDE46E0F9C14E9587, (363, "WOW-41089patch9.2.0_PTR", "Pet"));
            KeyInfo.Add(0x01A08F7CDA6E8F17, (364, "WOW-40078patch9.1.5_PTR", "Sylvanas epilogue"));
            KeyInfo.Add(0xB91972925E37456B, (365, "WOW-40078patch9.1.5_PTR", "Sylvanas epilogue"));
            KeyInfo.Add(0xB2708293A625EE14, (366, "WOW-40078patch9.1.5_PTR", "Dragon Isles intro vo/scene"));
            KeyInfo.Add(0x3E95E5966AB6E894, (367, "WOW-40290patch9.1.5_PTR", "Unknown Dragon cape"));
            KeyInfo.Add(0xB3A1B4341CFA4F4D, (368, "WOW-40622patch9.1.5_PTR", ""));
            KeyInfo.Add(0x8248F003C1441ADB, (369, "WOW-40622patch9.1.5_PTR", ""));
            KeyInfo.Add(0x48E73BE8D5EAF4B5, (370, "WOW-40622patch9.1.5_PTR", ""));
            KeyInfo.Add(0x8343AEF07B977C79, (371, "WOW-40622patch9.1.5_PTR", ""));
            KeyInfo.Add(0xDA9A6CB91E3D26B0, (372, "WOW-40622patch9.1.5_PTR", ""));
            KeyInfo.Add(0x1857CBD1054F94CC, (373, "WOW-42850patch9.2.5_PTR", "Quest/spell/in-game mail"));
            KeyInfo.Add(0xE791BF82C9499456, (374, "WOW-41360patch9.2.0_PTR", "Shattered Legacies cinematic"));
            KeyInfo.Add(0x8E202FAB57DFD542, (375, "WOW-41462patch9.2.0_PTR", "Unknown achievement"));
            KeyInfo.Add(0x37800F07285A7BC9, (376, "WOW-41462patch9.2.0_PTR", "Unknown achievement"));
            KeyInfo.Add(0x1B8739F92CDAD324, (377, "WOW-42850patch9.2.5_PTR", "Rabbit mount"));
            KeyInfo.Add(0x0947962739D1A90A, (378, "WOW-42850patch9.2.5_PTR", "Diablo 4 Amalgam of Rage"));
            KeyInfo.Add(0xCDD717BF616F1282, (379, "WOW-41962patch9.2.0_PTR", "Anduin Finale cinematic"));
            KeyInfo.Add(0xA8BBCCA0A621E518, (380, "WOW-41962patch9.2.0_PTR", "Jailer Intro cinematic"));
            KeyInfo.Add(0x7D38C6BF343CAB32, (381, "WOW-41962patch9.2.0_PTR", "Pelagos cinematic"));
            KeyInfo.Add(0xD01FFF442611F3C0, (382, "WOW-41962patch9.2.0_PTR", "Jailer Finale cinematic"));
            KeyInfo.Add(0x1E4534FEEC561A02, (384, "WOW-42850patch9.2.5_PTR", "fdid 841604, 1108759, 1237434, 1237435, 1237436, 1282621, 1720141"));
            KeyInfo.Add(0x1917CCB5CF95CD5E, (385, "WOW-42850patch9.2.5_PTR", "Trading post launch rewards"));
            KeyInfo.Add(0x77C4EB395B13E06C, (386, "WOW-42850patch9.2.5_PTR", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0xB9A5FE4AAF12D195, (387, "WOW-42850patch9.2.5_PTR", "Tirisfal story content "));
            KeyInfo.Add(0x50F45BF6BC2B387D, (390, "WOW-42850patch9.2.5_PTR", "Tuskarr campaign"));
            KeyInfo.Add(0xC25003980457691D, (391, "WOW-44649patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x103C4D80B653E8C5, (392, "WOW-42850patch9.2.5_PTR", "Anduin model"));
            KeyInfo.Add(0x7FC49C7CDE481335, (393, "WOW-42850patch9.2.5_PTR", "Tuskarr glider mount"));
            KeyInfo.Add(0x409FC8B6C2286508, (394, "WOW-43659patch3.4.0_Beta", "starts at fdid 841626, 107 files"));
            KeyInfo.Add(0x11103693800FE621, (395, "WOW-42850patch9.2.5_PTR", "Ichabod battle pet"));
            KeyInfo.Add(0x70AEC09B193DE12B, (396, "WOW-42850patch9.2.5_PTR", "Gooey Slimesaber Mount"));
            KeyInfo.Add(0xCAC9A9AD68A71FD3, (397, "WOW-42850patch9.2.5_PTR", "Bear cub battle pet"));
            KeyInfo.Add(0x6BBB68F40F470401, (398, "WOW-42850patch9.2.5_PTR", "Nethergorged greatwyrm mount"));
            KeyInfo.Add(0x7B7CF6626F5CF873, (399, "WOW-42850patch9.2.5_PTR", "Item set"));
            KeyInfo.Add(0x3146CA370908382D, (401, "WOW-42850patch9.2.5_PTR", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0xA2798DFCB1250E3E, (402, "WOW-44649patch10.0.0_Beta", "Telix the Stormhorn mount"));
            KeyInfo.Add(0x27C95702825CEEA9, (403, "WOW-43022patch9.2.5_PTR", "Queens gift cutscene"));
            KeyInfo.Add(0x4637768B740A71B7, (404, "WOW-44649patch10.0.0_Beta", "Dragon Isles intro scene/vo"));
            KeyInfo.Add(0x704D91D56DBB8DD9, (405, "WOW-43022patch9.2.5_PTR", "Kalecgos/Sindragosa cutscene"));
            KeyInfo.Add(0x068889361B441AAC, (413, "WOW-43810patch9.2.5_PTR", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x3C11A51D9EDF7946, (414, "WOW-43810patch9.2.5_PTR", "Drakks battle pet"));
            KeyInfo.Add(0x0A6701308BD0BCD6, (415, "WOW-44649patch10.0.0_Beta", "Wrathion/Alexstrazsa cutscene"));
            KeyInfo.Add(0xA4541C671097D0BD, (416, "WOW-44649patch10.0.0_Beta", "YSR scene"));
            KeyInfo.Add(0xF294CB91E66CDBA7, (417, "WOW-44649patch10.0.0_Beta", "Galakrond flashback cinematic"));
            KeyInfo.Add(0x5157D7ACE0AF2642, (418, "WOW-44649patch10.0.0_Beta", "Tyr friends cutscene"));
            KeyInfo.Add(0x3ACBD2001F88F34B, (419, "WOW-44649patch10.0.0_Beta", "starts at fdid 3483604, 103 files"));
            KeyInfo.Add(0xD289F4D4AF724C4D, (420, "WOW-44649patch10.0.0_Beta", "Unk 10.0 campaign db2 data"));
            KeyInfo.Add(0xD4C28A92DDFB407B, (421, "WOW-44649patch10.0.0_Beta", "Wrathion/Sabellian cutscene"));
            KeyInfo.Add(0x482F9F5FF11201C7, (422, "WOW-44649patch10.0.0_Beta", "Raszageth confrontation cutscene"));
            KeyInfo.Add(0x073344682FE80C0F, (423, "WOW-44649patch10.0.0_Beta", "Kalecgos blue flight cutscene"));
            KeyInfo.Add(0x73B2859937BDE2B1, (424, "WOW-44649patch10.0.0_Beta", "Tuskarr soup cutscene"));
            KeyInfo.Add(0xF7E1D001DC9D1339, (425, "WOW-44649patch10.0.0_Beta", "IGC Khadger/dragon council"));
            KeyInfo.Add(0xAE5194D30435C4FB, (426, "WOW-44649patch10.0.0_Beta", "Chromie/Nozdormu cutscene"));
            KeyInfo.Add(0xAA0C3B515E495282, (427, "WOW-44649patch10.0.0_Beta", "IGC Raszageth"));
            KeyInfo.Add(0x0D2AECE0D46D103C, (428, "WOW-47118patch10.0.5_PTR", "Recruit-a-friend v3 rewards"));
            KeyInfo.Add(0x583C5B29BF208655, (429, "WOW-44649patch10.0.0_Beta", "Unknown quest/spell/creature"));
            KeyInfo.Add(0x71046E55CD9DA78A, (431, "WOW-44795patch10.0.0_Beta", "fdid 841626, 1139443, 1260801, 1329070, 1572924"));
            KeyInfo.Add(0xBB2E41027A527755, (432, "WOW-44895patch10.0.0_Beta", "Frostbrood Proto-Wyrm mount"));
            KeyInfo.Add(0xF12E55A36ED24821, (434, "WOW-44930patch3.4.0_Beta", ""));
            KeyInfo.Add(0xBAAA5D1EFF565D36, (435, "WOW-44930patch3.4.0_Beta", "starts at fdid 841626, 72 files"));
            KeyInfo.Add(0xAF48C0D6BCD5A444, (437, "WOW-44999patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x7749CBD9B0C7B41C, (439, "WOW-45141patch10.0.0_Beta", "10.0 cinematics"));
            KeyInfo.Add(0xDE6E109D89A7F4D5, (440, "WOW-45141patch10.0.0_Beta", "Dracthyr warbringers cinematic"));
            KeyInfo.Add(0xFBE01D2C410E9A7C, (441, "WOW-45141patch10.0.0_Beta", "10.0 raid finale cinematic"));
            KeyInfo.Add(0x03BDE20E0001DB1F, (442, "WOW-45141patch10.0.0_Beta", "Alexstrasza Raszageth Fight cinematic"));
            KeyInfo.Add(0x9447A397D34E4A1E, (443, "WOW-45141patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0xD252BA4101AD4AF1, (444, "WOW-45232patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0xB9F65551C473F961, (445, "WOW-45232patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x80FF7EDA8451FFFE, (446, "WOW-45232patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x2F454B153E142B76, (469, "WOW-47118patch10.0.5_PTR", "Unknown creature"));
            KeyInfo.Add(0xB5F633819B0DE383, (470, "WOW-47118patch10.0.5_PTR", "Unknown creature"));
            KeyInfo.Add(0x1FFB6C550D055F12, (471, "WOW-49516patch10.1.5_VANPTR", ""));
            KeyInfo.Add(0x8C5276FE6714BE1C, (472, "WOW-49516patch10.1.5_VANPTR", ""));
            KeyInfo.Add(0x258D1178AB798566, (473, "WOW-45335patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x1E6AC96860BD524B, (474, "WOW-45335patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x6F5EBDF150B3441D, (475, "WOW-45335patch10.0.0_Beta", "Kalec Iskaara cutscene"));
            KeyInfo.Add(0x2252456E1E394005, (476, "WOW-45335patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x8526E0E4D352CD05, (477, "WOW-45335patch10.0.0_Beta", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0xDBB51BB7EBD83AD7, (478, "WOW-45569patch10.0.2_Beta", "Unknown spell/quest"));
            KeyInfo.Add(0xA119C0820C85C7C0, (479, "WOW-46368patch3.4.0_ClassicRetai", "Festering Emerald Drake"));
            KeyInfo.Add(0x153ECC70B31595F6, (480, "WOW-48480patch10.1.0_PTR", "79 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x617562110EE5B9DA, (481, "WOW-48520patch10.0.7_VANPTR", "50 file(s) in 10.0.7.48520"));
            KeyInfo.Add(0x08B3D95E2BE3957E, (482, "WOW-48520patch10.0.7_VANPTR", "0 file(s) in 10.0.7.48520"));
            KeyInfo.Add(0xFDB19DFAF1CFB214, (483, "WOW-46144patch10.0.2_Beta", "fdid 4867717, 4867718, 4867719, 4867720, 4867721"));
            KeyInfo.Add(0x024B699A3997EB72, (484, "WOW-46144patch10.0.2_Beta", "Dracthyr intro cinematic"));
            KeyInfo.Add(0x84F8A76EC1A549BF, (485, "WOW-46259patch10.0.2_Beta", "Anniversary store bundle"));
            KeyInfo.Add(0x625D0BAE331A3D20, (487, "WOW-46801patch10.0.2_Retail", "Tyr model"));
            KeyInfo.Add(0x32AA16A8A49B352D, (488, "WOW-47118patch10.0.5_PTR", "Trading post category"));
            KeyInfo.Add(0x1F2B201FCC73342F, (489, "WOW-47118patch10.0.5_PTR", "Trading post category"));
            KeyInfo.Add(0xA4838A9CE4760D1E, (491, "WOW-48520patch10.0.7_VANPTR", "Human heritage armor/related content"));
            KeyInfo.Add(0xC3699851FCBE7080, (492, "WOW-48520patch10.0.7_VANPTR", "Orc heritage armor/related content"));
            KeyInfo.Add(0xD98F0C2F9BE0F78D, (493, "WOW-48520patch10.0.7_VANPTR", "Unknown scene/creature/broadcasttext"));
            KeyInfo.Add(0x2D972D03742086AA, (494, "WOW-48520patch10.0.7_VANPTR", "Unknown quest/scene/zone/achievement/npcs"));
            KeyInfo.Add(0x209852729191E058, (495, "WOW-48520patch10.0.7_VANPTR", "Unknown quest/toy/item/achievement/mail"));
            KeyInfo.Add(0x398A71B6F3EB3779, (7055, "WOW-47118patch10.0.5_PTR", "Trading post content (April 2023)"));
            KeyInfo.Add(0x553C9B2CCBF1255F, (7058, "WOW-47118patch10.0.5_PTR", "Trading post content"));
            KeyInfo.Add(0x092562185FBD7B96, (7073, "WOW-47118patch10.0.5_PTR", "Trading post content"));
            KeyInfo.Add(0x6D591F2831A251A8, (7088, "WOW-47118patch10.0.5_PTR", "Trading post threshold"));
            KeyInfo.Add(0xAA360EBB4D552FCD, (7089, "WOW-47118patch10.0.5_PTR", "Trading post content"));
            KeyInfo.Add(0x31A110E087F8D181, (7092, "WOW-47118patch10.0.5_PTR", "Trading post content (March 2023)"));
            KeyInfo.Add(0x9FB918D19ED9C724, (7106, "WOW-47118patch10.0.5_PTR", "Trading post rewards"));
            KeyInfo.Add(0x4D58239760079E76, (7413, "WOW-47118patch10.0.5_PTR", "Trading post content"));
            KeyInfo.Add(0xEB68983BCFDD6DBD, (7421, "WOW-47118patch10.0.5_PTR", "Trading post activity"));
            KeyInfo.Add(0xA0CCE64398FF8992, (7430, "WOW-47118patch10.0.5_PTR", "Trading post activity"));
            KeyInfo.Add(0x6FDEBF276313ACD6, (7532, "WOW-47118patch10.0.5_PTR", "Unknown models"));
            KeyInfo.Add(0x860D713F28C4905E, (7533, "WOW-48520patch10.0.7_VANPTR", "Unknown scene/creature/broadcasttext"));
            KeyInfo.Add(0xF50C5E7920A90AC4, (7534, "WOW-48520patch10.0.7_VANPTR", "Secrets of the Reach cinematic"));
            KeyInfo.Add(0xF093CA07D3990E07, (7536, "WOW-47186patch10.0.5_PTR", "Unknown mount"));
            KeyInfo.Add(0x5C8B601AC2E8E3F8, (7537, "WOW-48520patch10.0.7_VANPTR", "Rocket Shredder 9001 mount"));
            KeyInfo.Add(0x7059D88EB22902D0, (7538, "WOW-48520patch10.0.7_VANPTR", "Unknown mount"));
            KeyInfo.Add(0x628E075788F3FE01, (7539, "WOW-48520patch10.0.7_VANPTR", "0 file(s) in 10.0.7.48520"));
            KeyInfo.Add(0xDB431E8EDB029366, (7540, "WOW-48520patch10.0.7_VANPTR", "0 file(s) in 10.0.7.48520"));
            KeyInfo.Add(0xCB117F47F4C8AC57, (7541, "WOW-48520patch10.0.7_VANPTR", "Recruit-a-friend v3 assets"));
            KeyInfo.Add(0xB0C155EC50F0939A, (7542, "WOW-48520patch10.0.7_VANPTR", "Baine questline/assets"));
            KeyInfo.Add(0x6288676A4F77D526, (7543, "WOW-48520patch10.0.7_VANPTR", "0 file(s) in 10.0.7.48520"));
            KeyInfo.Add(0xA3CCAD8A4B0FA129, (7544, "WOW-48520patch10.0.7_VANPTR", "Unknown model"));
            KeyInfo.Add(0x4758A4E70A0237AC, (7547, "WOW-48520patch10.0.7_VANPTR", "Unknown mount"));
            KeyInfo.Add(0x14BF95DCD8C32D85, (7548, "WOW-48480patch10.1.0_PTR", "Unknown map/quest/area/lfgdungeon"));
            KeyInfo.Add(0xF277310B18A1FFBA, (7549, "WOW-48520patch10.0.7_VANPTR", "Battle.net pet bundle assets"));
            KeyInfo.Add(0xB5147B68B5E93830, (7550, "WOW-48480patch10.1.0_PTR", "Evoker legendary"));
            KeyInfo.Add(0xF2DC126FEB572258, (7551, "WOW-48480patch10.1.0_PTR", "Opening the Way cinematic"));
            KeyInfo.Add(0x632946FAD80476A1, (7552, "WOW-48480patch10.1.0_PTR", "Unknown item/spells/questline"));
            KeyInfo.Add(0xF050A0D99707361C, (7553, "WOW-48480patch10.1.0_PTR", "Unknown quest"));
            KeyInfo.Add(0xB2A7A2FB56623E73, (7554, "WOW-48480patch10.1.0_PTR", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0x9A4EF7CC292DE96C, (7555, "WOW-48480patch10.1.0_PTR", "wtr cinematic"));
            KeyInfo.Add(0x4E538E4958741434, (7556, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x665F797C04DC41FD, (7557, "WOW-48480patch10.1.0_PTR", "The Last Conflict scene"));
            KeyInfo.Add(0x6BC314BEB3DA0876, (7558, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x2678EB9E5884B351, (7559, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0xDB8744326BF97458, (7560, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0xE2C1F6229F75B1FB, (7561, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x60ED94F2D835847F, (7562, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x6840763425368ACC, (7563, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x27974D1E554D2362, (7564, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x3FD6396835D88289, (7565, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x9E5AACFD4E1BEC58, (7566, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext/vo"));
            KeyInfo.Add(0x9F4F14E8B03E153C, (7567, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x892C9D2191600761, (7568, "WOW-48480patch10.1.0_PTR", "Unknown sounds"));
            KeyInfo.Add(0x8FB1CD0942DDA16D, (7569, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0xF381DD2EEFD641F2, (7570, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0x4CB34AC33ED26838, (7571, "WOW-48480patch10.1.0_PTR", "Unknown sounds"));
            KeyInfo.Add(0x056E62CF9455648F, (7572, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext/vo"));
            KeyInfo.Add(0x0240393B73CF0D29, (7573, "WOW-48480patch10.1.0_PTR", "Sarkareth raid finale"));
            KeyInfo.Add(0xBD0E7EA85857598E, (7574, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0xEBE224C64720594B, (7575, "WOW-48480patch10.1.0_PTR", "Unknown broadcasttext"));
            KeyInfo.Add(0xB25044202A4BD9B7, (7576, "WOW-48480patch10.1.0_PTR", "0 file(s) as of 10.1.0.48480"));
            KeyInfo.Add(0xAC86DE77F65B93AF, (7577, "WOW-48520patch10.0.7_VANPTR", "Waveborne Diplomat's Regalia transmog set"));
            KeyInfo.Add(0x0FAB680C2AC0E38E, (7587, "WOW-49516patch10.1.5_VANPTR", ""));
            KeyInfo.Add(0xC85DA43484057737, (7588, "WOW-50442patch10.1.7_PTR", "103 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x78C70CF0F282D257, (7589, "WOW-48776patch10.1.0_PTR", "Unknown model"));
            KeyInfo.Add(0x2DFCA73AC64AEECF, (7590, "WOW-48776patch10.1.0_PTR", "Unknown items (?)"));
            KeyInfo.Add(0xE8175532FAA4D0D7, (7591, "WOW-48776patch10.1.0_PTR", "Unknown model"));
            KeyInfo.Add(0x6C36AE2EE5E4B9DC, (7592, "WOW-48776patch10.1.0_PTR", "Unknown model"));
            KeyInfo.Add(0x0599D267A15C719F, (7593, "WOW-48776patch10.1.0_PTR", "Unknown textures"));
            KeyInfo.Add(0x4CE29C24FDFDFB18, (7594, "WOW-48776patch10.1.0_PTR", "Guardian transmog set"));
            KeyInfo.Add(0x2D71F7DD989FED1F, (7595, "WOW-48776patch10.1.0_PTR", "Turtle Mount"));
            KeyInfo.Add(0xC4B1348F2B37B3B4, (7596, "WOW-48776patch10.1.0_PTR", "Blue dragonflight campaign timer"));
            KeyInfo.Add(0xA765D5EDE5034CFA, (7597, "WOW-48898patch10.1.0_PTR", "Trading post content"));
            KeyInfo.Add(0x468AFCD3B345C56C, (7598, "WOW-48898patch10.1.0_PTR", "0 file(s) as of 10.1.0.48898"));
            KeyInfo.Add(0xFAE576DB8432F32E, (7599, "WOW-48898patch10.1.0_PTR", "0 file(s) as of 10.1.0.48898"));
            KeyInfo.Add(0xB3DA923C2F13FD5B, (7600, "WOW-48898patch10.1.0_PTR", "0 file(s) as of 10.1.0.48898"));
            KeyInfo.Add(0x4EAD7F8A0C378D7F, (7601, "WOW-48898patch10.1.0_PTR", "0 file(s) as of 10.1.0.48898"));
            KeyInfo.Add(0x6899BB495C433867, (7602, "WOW-48865patch10.0.7_Retail", "Unknown store assets"));
            KeyInfo.Add(0x69CB2A2C597E9F67, (7603, "WOW-48865patch10.0.7_Retail", "Faction pack store assets"));
            KeyInfo.Add(0xAF8553E077BA216C, (7605, "WOW-49516patch10.1.5_VANPTR", "Unknown broadcasttext/model"));
            KeyInfo.Add(0x2337416B8A912391, (7606, "WOW-49516patch10.1.5_VANPTR", "Netherwing companion drake (teal)"));
            KeyInfo.Add(0x5468F352E97569AE, (7607, "WOW-50442patch10.1.7_PTR", "115 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0xD4A78D149406F94F, (7608, "WOW-49516patch10.1.5_VANPTR", "Unknown toy"));
            KeyInfo.Add(0x65347C95BA2C6634, (7610, "WOW-49516patch10.1.5_VANPTR", "Unknown item/spell/quest"));
            KeyInfo.Add(0x65394BA0A4F0509A, (7612, "WOW-50442patch10.1.7_PTR", "4 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x42C1954DF757827B, (7613, "WOW-49516patch10.1.5_VANPTR", "1 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x3722BAA1FE7809C5, (7614, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0xAA6A7C5C5ED8DBC6, (7615, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x68AA63A16E5EF2B1, (7616, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x4AD2E2A4B055E925, (7617, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0xA9E5F1F6208B98B8, (7618, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x95F0FD06F2909D19, (7619, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0xC9BA30DA927A14B9, (7620, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x4C9249E0480C8264, (7621, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x6A552F8A1D3B8637, (7622, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x5AA7FAB564B04D7D, (7623, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0xE8A4042B5D3FD25D, (7624, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x1D6294FC250DF4A1, (7625, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0xCC8D5FD991958339, (7626, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x8553C9D58E44FD19, (7627, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x4626421172DFF459, (7628, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x81EEFCCFC9DF862A, (7629, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x38CB18F712F9C63E, (7630, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0xEE0541B6CAF0AB77, (7631, "WOW-49516patch10.1.5_VANPTR", "5 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x9DDF72DFC13DFBD3, (7632, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x26947FF164DA0962, (7633, "WOW-49516patch10.1.5_VANPTR", "6 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x4028F29E55EA5247, (7634, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x5DBE4562C104CCBE, (7635, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x3923F1B9935BA8FA, (7636, "WOW-49516patch10.1.5_VANPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x70B9EBFAB71855B5, (7638, "WOW-50442patch10.1.7_PTR", "33 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x724CFA81FCC5334C, (7639, "WOW-49595patch10.1.5_XPTR", "0 file(s) as of 10.1.5.49595"));
            KeyInfo.Add(0x5F21546AEF75EAAA, (7641, "WOW-50199patch10.1.5_XPTR", "4 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xCEEEEBD0D1C95671, (7642, "WOW-50442patch10.1.7_PTR", "0 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x8AF3FBB11B5AEA5A, (7643, "WOW-50442patch10.1.7_PTR", "22 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x512EB72CDF6ABDA2, (7644, "WOW-50442patch10.1.7_PTR", "34 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x0C34FD02900A67D6, (7646, "WOW-50442patch10.1.7_PTR", "10 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xDB08C8015AE584F9, (7647, "WOW-50199patch10.1.5_XPTR", "10 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x7A2BDD5F02385D8B, (7648, "WOW-50199patch10.1.5_XPTR", "379 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xCE967C5C045FA50A, (7651, "WOW-50199patch10.1.5_XPTR", "17 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x9C348A0A912745F6, (7652, "WOW-50442patch10.1.7_PTR", "93 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x312822946C76E11C, (7653, "WOW-50442patch10.1.7_PTR", "64 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0xFEA26F852E83A774, (7654, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xBFA213ABA4804E4C, (7655, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xF87C86A36878B72D, (7656, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xA9B4E4CB5F651DB0, (7657, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x3E443B900EA0D9AD, (7658, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xA919AA460BEE447A, (7659, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x37A4D8DCD8710EC2, (7660, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x843AE002EC839322, (7661, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x460225C73E8F3DA5, (7662, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x994EEC72BF7677B8, (7663, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x8D0F298BD7CCD04C, (7664, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x06462A626C07CFC0, (7665, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xFCE98E3A1E7E6ACC, (7666, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x9FE4383ED8CD685C, (7667, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x1638836B18413FA1, (7668, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xCC9BC67D426D0109, (7669, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x508A7CA469D3ABD4, (7670, "WOW-50199patch10.1.5_XPTR", "0 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xEBEAAC307E3D823F, (7671, "WOW-50442patch10.1.7_PTR", "41 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0xC26B17EC8991C3DE, (7672, "WOW-50199patch10.1.5_XPTR", "18 file(s) as of 10.1.5.50199"));
            KeyInfo.Add(0x5D3122DA6FBBA7F2, (7673, "WOW-50442patch10.1.7_PTR", "88 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0xEAB45D6B59CDCE29, (7675, "WOW-50199patch10.1.5_XPTR", "18 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x614FA5E9FE31B27A, (7676, "WOW-50199patch10.1.5_XPTR", "7 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x8DB2A67206C3BBFE, (7677, "WOW-50199patch10.1.5_XPTR", "4 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x8B67488DE1E962A7, (7678, "WOW-50199patch10.1.5_XPTR", "4 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x4E3AA2CB1B379256, (7679, "WOW-50199patch10.1.5_XPTR", "4 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x04132CC1A52E9DD3, (7680, "WOW-50199patch10.1.5_XPTR", "4 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x74FFB4F6918309D6, (7681, "WOW-50442patch10.1.7_PTR", "8 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x3D7F53C63B044491, (7682, "WOW-50442patch10.1.7_PTR", "73 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x7F070B1D792933D7, (7686, "WOW-50442patch10.1.7_PTR", "15 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0xCFFE42581EF7E189, (7688, "WOW-50438patch10.1.5_XPTR", "16 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x972EFE7B2D050C9C, (7689, "WOW-50438patch10.1.5_XPTR", "7 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x3C2909D607694F17, (7690, "WOW-50438patch10.1.5_XPTR", "7 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x62F7CF550FD52D4B, (7691, "WOW-50442patch10.1.7_PTR", "0 file(s) as of 10.1.7.50442"));
            KeyInfo.Add(0x2CBBC28266D3782D, (7697, "WOW-50505patch10.1.7_PTR", "9 file(s) as of 10.1.7.50505"));
        }

        private class KeyInfoEntry
        {
            public int ID { get; set; }
            public string Lookup { get; set; }
            public string Key { get; set; }
            public string FirstSeen { get; set; }
            public string Description { get; set; }
        }

        [Route("info")]
        [HttpGet]
        public string Info()
        {
            var tklStorage = dbcManager.GetOrLoad("TactKeyLookup", CASC.BuildName, true).Result;

            foreach (dynamic tklRow in tklStorage.Values)
            {
                ulong key = BitConverter.ToUInt64(tklRow.TACTID);

                if (!KeyInfo.ContainsKey(key))
                {
                    KeyInfo.Add(
                        key,
                        ((int)tklRow.ID,
                        CASC.GetFullBuild(),
                        CASC.EncryptedFDIDs.Where(x => x.Value.Contains(key)).Select(x => x.Key).ToList().Count.ToString() + " file(s) as of " + CASC.BuildName)
                    );
                }
                else if (KeyInfo[key].Description == "" || KeyInfo[key].Description.Contains("file(s) as of"))
                {
                    var copy = KeyInfo[key];
                    copy.Description = CASC.EncryptedFDIDs.Where(x => x.Value.Contains(key)).Select(x => x.Key).ToList().Count.ToString() + " file(s) as of " + CASC.BuildName;
                    KeyInfo[key] = copy;
                }
            }

            var tkStorage = dbcManager.GetOrLoad("TactKey", CASC.BuildName, true).Result;
            foreach (dynamic tkRow in tkStorage.Values)
            {
                foreach (var keyInfo in KeyInfo)
                {
                    if (keyInfo.Value.ID != (int)tkRow.ID)
                        continue;

                    if (!CASC.KnownKeys.Contains(keyInfo.Key))
                        CASC.KnownKeys.Add(keyInfo.Key);

                    if (KeyService.HasKey(keyInfo.Key))
                        continue;


                    Console.WriteLine("Setting key " + (int)tkRow.ID + " from TactKey.db2");
                    KeyService.SetKey(keyInfo.Key, tkRow.Key);
                }
            }

            if (Directory.Exists("caches"))
            {
                foreach (var file in Directory.GetFiles("caches", "*.bin", SearchOption.AllDirectories))
                {
                    var cache = new DBCacheParser(file);

                    foreach (var hotfix in cache.hotfixes)
                    {
                        if (hotfix.dataSize == 0) continue;

                        if (hotfix.tableHash == 0x021826BB)
                        {
                            using (var ms = new MemoryStream(hotfix.data))
                            using (var bin = new BinaryReader(ms))
                            {
                                bin.ReadCString(); // Text_lang
                                bin.ReadCString(); // Text1_lang
                                bin.ReadUInt32();  // ID
                                bin.ReadUInt32();  // LanguageID
                                bin.ReadUInt32();  // ConditionID
                                bin.ReadUInt16();  // EmotesID
                                bin.ReadByte();    // Flags
                                bin.ReadUInt32();  // ChatBubbleDurationMs
                                bin.ReadUInt32();  // VoiceOverPriorityID
                                bin.ReadUInt32();  // SoundKitID[0]
                                bin.ReadUInt32();  // SoundKitID[1]
                                bin.ReadUInt16();  // EmoteID[0]
                                bin.ReadUInt16();  // EmoteID[1]
                                bin.ReadUInt16();  // EmoteID[2]
                                bin.ReadUInt16();  // EmoteDelay[0]
                                bin.ReadUInt16();  // EmoteDelay[1]
                                bin.ReadUInt16();  // EmoteDelay[2]
                                if (bin.BaseStream.Position != bin.BaseStream.Length)
                                {

                                    var extraTableHash = bin.ReadUInt32();
                                    if (extraTableHash == 0xDF2F53CF)
                                    {
                                        var tactKeyLookup = bin.ReadUInt64();
                                        var tactKeyBytes = bin.ReadBytes(16);

                                        if (KeyService.HasKey(tactKeyLookup))
                                            continue;

                                        CASC.KnownKeys.Add(tactKeyLookup);

                                        KeyService.SetKey(tactKeyLookup, tactKeyBytes);

                                        Console.WriteLine("Found TACT Key " + string.Format("{0:X}", tactKeyLookup).PadLeft(16, '0') + " " + Convert.ToHexString(tactKeyBytes));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (SettingsManager.wowFolder != null)
            {
                foreach (var file in Directory.GetFiles(SettingsManager.wowFolder, "DBCache.bin", SearchOption.AllDirectories))
                {
                    var cache = new DBCacheParser(file);

                    foreach (var hotfix in cache.hotfixes)
                    {
                        if (hotfix.dataSize == 0) continue;

                        if (hotfix.tableHash == 0x021826BB)
                        {
                            using (var ms = new MemoryStream(hotfix.data))
                            using (var bin = new BinaryReader(ms))
                            {
                                bin.ReadCString(); // Text_lang
                                bin.ReadCString(); // Text1_lang
                                bin.ReadUInt32();  // ID
                                bin.ReadUInt32();  // LanguageID
                                bin.ReadUInt32();  // ConditionID
                                bin.ReadUInt16();  // EmotesID
                                bin.ReadByte();    // Flags
                                bin.ReadUInt32();  // ChatBubbleDurationMs
                                bin.ReadUInt32();  // VoiceOverPriorityID
                                bin.ReadUInt32();  // SoundKitID[0]
                                bin.ReadUInt32();  // SoundKitID[1]
                                bin.ReadUInt16();  // EmoteID[0]
                                bin.ReadUInt16();  // EmoteID[1]
                                bin.ReadUInt16();  // EmoteID[2]
                                bin.ReadUInt16();  // EmoteDelay[0]
                                bin.ReadUInt16();  // EmoteDelay[1]
                                bin.ReadUInt16();  // EmoteDelay[2]
                                if (bin.BaseStream.Position != bin.BaseStream.Length)
                                {

                                    var extraTableHash = bin.ReadUInt32();
                                    if (extraTableHash == 0xDF2F53CF)
                                    {
                                        var tactKeyLookup = bin.ReadUInt64();
                                        var tactKeyBytes = bin.ReadBytes(16);

                                        if (KeyService.HasKey(tactKeyLookup))
                                            continue;

                                        CASC.KnownKeys.Add(tactKeyLookup);

                                        KeyService.SetKey(tactKeyLookup, tactKeyBytes);

                                        Console.WriteLine("Found TACT Key " + string.Format("{0:X}", tactKeyLookup).PadLeft(16, '0') + " " + Convert.ToHexString(tactKeyBytes));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var keyInfos = new List<KeyInfoEntry>();

            foreach (var keyInfo in KeyInfo)
            {
                keyInfos.Add(
                   new KeyInfoEntry
                   {
                       ID = keyInfo.Value.ID,
                       Lookup = string.Format("{0:X}", keyInfo.Key).PadLeft(16, '0'),
                       Key = KeyService.HasKey(keyInfo.Key) ? Convert.ToHexString(KeyService.GetKey(keyInfo.Key)) : "",
                       FirstSeen = keyInfo.Value.FirstSeen,
                       Description = keyInfo.Value.Description
                   });
            }

            return JsonSerializer.Serialize(keyInfos.OrderBy(x => x.ID));
        }
    }
}
