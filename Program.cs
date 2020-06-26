using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel.Design;

namespace monster_db_extract
{
    class Program
    {
        // List of monster spawns
        public static List<Spawn> spawns = new List<Spawn>();
        public static int total_spawns = AmountOfSpawns();
        public static string output = "";

        // List of already used coordinates for spawns
        public static List<Coordinate> coordinates = new List<Coordinate>();

        // Offset spawn order (0-17)
        public static List<int> offset_x = new List<int> { 0, 1, 1, 0, -1, -1, -1, 0, 1, 2, 2, 2, 2, 1, 0, -1, -2, -2 };
        public static List<int> offset_y = new List<int> { 0, 0, -1, -1, -1, 0, 1, 1, 1, 1, 0, -1, -2, -2, -2, -2, -2, -1 };
        public static int current_offset = 0;

        // List of item id's that are non-walkable
        public static List<string> invalid_items = new List<string> { "100", "101", "293", "294", "356", "357", "358", "359", "360", "361", "362", "363", "364", "365", "366", "367", "369", "370", "371", "373", "374", "375", "376", "377", "378", "379", "380", "381", "382", "383", "384", "385", "388", "389", "390", "391", "392", "393", "394", "411", "412", "413", "414", "419", "426", "427", "428", "430", "431", "432", "433", "434", "437", "438", "439", "440", "441", "442", "443", "444", "445", "446", "447", "448", "449", "450", "451", "452", "465", "466", "467", "471", "472", "473", "474", "475", "476", "477", "478", "480", "482", "483", "484", "485", "487", "488", "489", "490", "491", "492", "493", "494", "495", "496", "497", "498", "563", "566", "567", "568", "569", "570", "589", "590", "591", "592", "594", "595", "599", "600", "601", "604", "605", "607", "609", "610", "615", "618", "619", "620", "621", "622", "623", "624", "625", "626", "627", "628", "629", "630", "631", "632", "633", "634", "635", "636", "637", "638", "639", "640", "641", "642", "643", "644", "645", "646", "647", "648", "649", "650", "651", "652", "653", "654", "655", "656", "657", "658", "659", "661", "663", "667", "668", "670", "671", "672", "673", "674", "675", "676", "677", "678", "679", "680", "681", "682", "683", "684", "685", "686", "687", "688", "689", "690", "691", "692", "693", "694", "695", "696", "697", "698", "699", "700", "701", "702", "703", "704", "705", "706", "707", "708", "709", "710", "711", "712", "713", "714", "715", "716", "717", "718", "719", "720", "723", "724", "727", "728", "729", "730", "731", "732", "733", "734", "735", "736", "737", "738", "739", "740", "741", "742", "743", "744", "745", "746", "747", "748", "749", "750", "751", "752", "753", "754", "755", "756", "757", "758", "759", "760", "761", "762", "763", "764", "765", "766", "767", "769", "771", "775", "776", "777", "781", "782", "784", "786", "787", "788", "789", "790", "791", "792", "793", "794", "795", "796", "797", "798", "837", "838", "839", "840", "841", "842", "843", "844", "845", "846", "847", "848", "849", "850", "851", "852", "853", "854", "855", "856", "857", "858", "859", "860", "861", "862", "863", "864", "1066", "1067", "1079", "1080", "1081", "1082", "1083", "1084", "1085", "1086", "1099", "1112", "1113", "1114", "1115", "1116", "1117", "1118", "1119", "1120", "1121", "1122", "1123", "1124", "1125", "1126", "1127", "1128", "1129", "1130", "1131", "1132", "1133", "1134", "1135", "1136", "1137", "1138", "1139", "1140", "1141", "1142", "1143", "1144", "1145", "1146", "1147", "1148", "1149", "1150", "1151", "1153", "1154", "1155", "1156", "1157", "1160", "1162", "1163", "1164", "1165", "1166", "1167", "1170", "1172", "1173", "1174", "1175", "1176", "1177", "1181", "1182", "1183", "1184", "1185", "1186", "1187", "1191", "1192", "1193", "1194", "1195", "1196", "1198", "1199", "1200", "1201", "1202", "1203", "1204", "1205", "1206", "1207", "1208", "1209", "1270", "1271", "1272", "1273", "1274", "1275", "1276", "1277", "1278", "1279", "1280", "1281", "1282", "1283", "1284", "1285", "1286", "1287", "1288", "1289", "1290", "1291", "1292", "1293", "1294", "1295", "1296", "1297", "1298", "1299", "1300", "1301", "1302", "1303", "1304", "1305", "1306", "1307", "1308", "1309", "1310", "1311", "1312", "1313", "1314", "1315", "1316", "1329", "1330", "1331", "1332", "1333", "1334", "1335", "1336", "1337", "1338", "1339", "1340", "1341", "1342", "1343", "1344", "1345", "1346", "1347", "1348", "1349", "1350", "1351", "1352", "1353", "1354", "1355", "1356", "1357", "1358", "1359", "1360", "1361", "1362", "1363", "1364", "1365", "1366", "1367", "1368", "1369", "1370", "1371", "1372", "1373", "1374", "1375", "1376", "1377", "1378", "1379", "1380", "1381", "1382", "1383", "1384", "1385", "1386", "1387", "1388", "1389", "1390", "1391", "1392", "1393", "1394", "1395", "1396", "1397", "1398", "1399", "1400", "1401", "1402", "1403", "1404", "1405", "1406", "1407", "1408", "1409", "1410", "1411", "1412", "1413", "1414", "1415", "1416", "1417", "1418", "1419", "1420", "1421", "1422", "1423", "1424", "1425", "1426", "1427", "1428", "1429", "1430", "1431", "1432", "1433", "1434", "1435", "1436", "1437", "1438", "1439", "1440", "1441", "1442", "1443", "1444", "1445", "1446", "1447", "1448", "1449", "1450", "1451", "1452", "1453", "1454", "1455", "1456", "1457", "1458", "1459", "1460", "1461", "1462", "1463", "1464", "1465", "1466", "1467", "1468", "1469", "1470", "1471", "1472", "1473", "1474", "1475", "1476", "1477", "1478", "1479", "1480", "1481", "1482", "1483", "1484", "1485", "1486", "1487", "1488", "1489", "1490", "1491", "1492", "1493", "1494", "1495", "1496", "1497", "1498", "1499", "1500", "1501", "1502", "1503", "1504", "1505", "1506", "1507", "1508", "1509", "1510", "1511", "1512", "1513", "1514", "1515", "1516", "1517", "1518", "1519", "1520", "1521", "1522", "1523", "1524", "1525", "1526", "1527", "1528", "1529", "1530", "1531", "1532", "1533", "1534", "1535", "1536", "1537", "1538", "1539", "1540", "1541", "1542", "1543", "1544", "1545", "1546", "1547", "1548", "1549", "1550", "1551", "1552", "1553", "1554", "1555", "1556", "1557", "1558", "1559", "1560", "1561", "1562", "1563", "1564", "1565", "1566", "1567", "1568", "1569", "1570", "1571", "1572", "1573", "1574", "1575", "1576", "1577", "1578", "1579", "1580", "1581", "1582", "1583", "1584", "1585", "1586", "1587", "1588", "1589", "1590", "1591", "1592", "1593", "1594", "1595", "1596", "1597", "1598", "1599", "1600", "1601", "1602", "1603", "1604", "1605", "1606", "1607", "1608", "1609", "1610", "1611", "1612", "1613", "1614", "1615", "1616", "1617", "1618", "1619", "1620", "1621", "1622", "1623", "1624", "1625", "1626", "1627", "1628", "1629", "1630", "1631", "1632", "1633", "1634", "1635", "1636", "1637", "1638", "1639", "1640", "1641", "1642", "1643", "1644", "1645", "1646", "1647", "1648", "1649", "1650", "1651", "1652", "1653", "1654", "1655", "1656", "1657", "1658", "1659", "1660", "1661", "1662", "1663", "1664", "1665", "1666", "1667", "1668", "1669", "1670", "1671", "1672", "1673", "1674", "1675", "1676", "1677", "1678", "1679", "1680", "1681", "1682", "1683", "1684", "1685", "1686", "1687", "1688", "1689", "1690", "1691", "1692", "1693", "1694", "1695", "1696", "1697", "1698", "1699", "1700", "1701", "1702", "1703", "1704", "1705", "1706", "1707", "1708", "1709", "1710", "1711", "1712", "1713", "1714", "1715", "1718", "1719", "1720", "1721", "1722", "1723", "1724", "1725", "1726", "1727", "1728", "1729", "1730", "1731", "1732", "1733", "1734", "1735", "1736", "1737", "1738", "1739", "1740", "1741", "1742", "1743", "1744", "1745", "1746", "1747", "1748", "1749", "1750", "1751", "1752", "1753", "1754", "1765", "1766", "1769", "1770", "1772", "1773", "1774", "1775", "1776", "1777", "1778", "1779", "1783", "1784", "1785", "1786", "1787", "1788", "1789", "1790", "1791", "1792", "1793", "1794", "1796", "1797", "1799", "1801", "1803", "1804", "1806", "1808", "1810", "1811", "1813", "1815", "1817", "1818", "1820", "1822", "1824", "1825", "1826", "1827", "1828", "1829", "1830", "1831", "1832", "1833", "1834", "1835", "1836", "1837", "1838", "1839", "1840", "1841", "1842", "1843", "1844", "1845", "1846", "1847", "1848", "1849", "1850", "1851", "1852", "1853", "1859", "1860", "1861", "1862", "1863", "1864", "1868", "1869", "1870", "1871", "1872", "1873", "1874", "1875", "1876", "1877", "1878", "1879", "1880", "1881", "1882", "1883", "1884", "1885", "1886", "1887", "1888", "1889", "1890", "1891", "1901", "1902", "1903", "1904", "1905", "1906", "1907", "1908", "1909", "1910", "1911", "1912", "1913", "1914", "1915", "1916", "1917", "1918", "1919", "1920", "1921", "1922", "1923", "1924", "1925", "1926", "1927", "1928", "1929", "1930", "1931", "1932", "1933", "1934", "1935", "1936", "1937", "1938", "1939", "1940", "1941", "1942", "1943", "1944", "1945", "1946", "1947", "1949", "1950", "1951", "1952", "1953", "1954", "1955", "1956", "1957", "1958", "1959", "1960", "1961", "1962", "1963", "1964", "1965", "1966", "1967", "1969", "1970", "1971", "1972", "1973", "1974", "1975", "1976", "1977", "1978", "1980", "1981", "1983", "1984", "1985", "1986", "1987", "1990", "1991", "1992", "1993", "1994", "1995", "1998", "1999", "2000", "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2025", "2026", "2027", "2028", "2029", "2030", "2031", "2032", "2033", "2034", "2035", "2036", "2037", "2038", "2039", "2040", "2041", "2042", "2043", "2044", "2045", "2046", "2047", "2048", "2049", "2050", "2051", "2052", "2053", "2054", "2059", "2060", "2061", "2062", "2063", "2064", "2065", "2066", "2067", "2068", "2069", "2070", "2071", "2072", "2073", "2074", "2075", "2076", "2077", "2078", "2079", "2080", "2081", "2082", "2083", "2084", "2085", "2086", "2087", "2088", "2089", "2090", "2091", "2092", "2093", "2094", "2095", "2096", "2097", "2098", "2099", "2100", "2101", "2102", "2103", "2104", "2105", "2106", "2107", "2108", "2109", "2110", "2111", "2112", "2113", "2114", "2115", "2116", "2117", "2118", "2119", "2120", "2121", "2122", "2123", "2124", "2125", "2126", "2127", "2128", "2129", "2130", "2131", "2132", "2133", "2134", "2135", "2136", "2137", "2138", "2141", "2142", "2143", "2145", "2146", "2148", "2149", "2150", "2151", "2152", "2153", "2154", "2155", "2156", "2157", "2158", "2159", "2160", "2161", "2162", "2163", "2164", "2165", "2166", "2167", "2168", "2169", "2170", "2171", "2172", "2173", "2174", "2175", "2176", "2177", "2178", "2179", "2180", "2181", "2182", "2183", "2184", "2185", "2186", "2187", "2188", "2189", "2190", "2191", "2192", "2193", "2194", "2195", "2196", "2197", "2198", "2199", "2200", "2201", "2202", "2203", "2204", "2205", "2206", "2207", "2208", "2209", "2210", "2211", "2212", "2213", "2214", "2215", "2216", "2217", "2218", "2219", "2220", "2221", "2222", "2223", "2224", "2225", "2226", "2227", "2228", "2229", "2230", "2231", "2232", "2233", "2234", "2235", "2236", "2237", "2238", "2239", "2240", "2241", "2242", "2243", "2244", "2245", "2246", "2247", "2248", "2249", "2250", "2251", "2252", "2255", "2256", "2257", "2258", "2263", "2264", "2265", "2266", "2267", "2268", "2269", "2270", "2271", "2272", "2273", "2274", "2275", "2276", "2277", "2278", "2279", "2280", "2281", "2282", "2283", "2284", "2285", "2286", "2287", "2288", "2289", "2290", "2291", "2292", "2293", "2294", "2295", "2296", "2297", "2298", "2299", "2300", "2301", "2302", "2303", "2304", "2305", "2306", "2307", "2308", "2309", "2310", "2311", "2312", "2313", "2314", "2315", "2316", "2317", "2318", "2319", "2320", "2321", "2322", "2323", "2324", "2325", "2326", "2327", "2328", "2329", "2330", "2331", "2332", "2333", "2334", "2336", "2338", "2339", "2340", "2341", "2342", "2343", "2344", "2345", "2346", "2347", "2348", "2349", "2350", "2351", "2352", "2353", "2354", "2355", "2356", "2357", "2358", "2359", "2360", "2361", "2362", "2363", "2364", "2365", "2366", "2367", "2368", "2369", "2370", "2371", "2372", "2373", "2374", "2375", "2376", "2377", "2378", "2379", "2380", "2381", "2382", "2383", "2384", "2385", "2402", "2403", "2404", "2405", "2406", "2407", "2408", "2409", "2410", "2411", "2412", "2413", "2414", "2415", "2416", "2417", "2418", "2419", "2420", "2421", "2422", "2423", "2424", "2425", "2426", "2427", "2428", "2429", "2430", "2431", "2432", "2433", "2434", "2435", "2436", "2437", "2438", "2439", "2440", "2441", "2442", "2443", "2444", "2445", "2446", "2447", "2448", "2449", "2450", "2451", "2452", "2453", "2454", "2455", "2456", "2457", "2458", "2459", "2460", "2461", "2462", "2463", "2464", "2465", "2466", "2467", "2468", "2469", "2470", "2471", "2472", "2473", "2474", "2475", "2476", "2477", "2478", "2479", "2480", "2481", "2482", "2483", "2484", "2485", "2486", "2487", "2488", "2489", "2490", "2491", "2492", "2493", "2494", "2495", "2496", "2497", "2498", "2499", "2500", "2501", "2502", "2503", "2504", "2505", "2506", "2507", "2508", "2509", "2510", "2519", "2520", "2521", "2522", "2523", "2524", "2525", "2526", "2527", "2528", "2529", "2530", "2531", "2532", "2533", "2534", "2535", "2536", "2537", "2538", "2539", "2540", "2541", "2542", "2543", "2544", "2545", "2546", "2547", "2548", "2549", "2550", "2551", "2552", "2553", "2554", "2555", "2556", "2557", "2558", "2559", "2560", "2561", "2562", "2563", "2564", "2565", "2568", "2571", "2738", "2739", "2740", "2741", "2742", "2743", "2767", "2768", "2771", "2775", "2776", "2777", "2778", "2779", "2780", "2781", "2782", "2783", "2784", "2785", "2786", "2787", "2788", "2789", "2790", "2791", "2792", "2793", "2794", "2795", "2796", "2797", "2798", "2799", "2800", "2801", "2802", "2803", "2804", "2805", "2806", "2807", "2808", "2809", "2810", "2811", "2812", "2853", "2854", "2855", "2856", "2857", "2858", "2859", "2860", "2861", "2862", "2863", "2864", "2865", "2866", "2867", "2868", "2869", "2870", "2871", "2872", "2873", "2874", "2875", "2876", "2877", "2879", "2880", "2881", "2882", "2883", "2884", "2885", "2893", "2901", "2902", "2903", "2904", "2959", "2960", "2961", "2962", "2963", "2964", "2974", "2975", "2976", "2979", "2980", "2982", "2985", "2986", "2987", "2997", "2998", "2999", "3000", "3221", "3244", "3255", "3256", "3257", "3258", "3259", "3260", "3261", "3262", "3263", "3458", "3465", "3477", "3478", "3479", "3480", "3482", "3484", "3485", "3486", "3487", "3488", "3489", "3490", "3491", "3493", "3494", "3495", "3496", "3497", "3498", "3499", "3500", "3501", "3502", "3503", "3504", "3508", "3510", "3511", "3512", "3513", "3514", "3515", "3516", "3517", "3518", "3519", "3520", "3521", "3522", "3523", "3524", "3525", "3526", "3527", "3528", "3529", "3530", "3531", "3532", "3608", "3609", "3610", "3611", "3612", "3613", "3614", "3615", "3616", "3617", "3618", "3619", "3620", "3621", "3622", "3623", "3624", "3625", "3626", "3627", "3628", "3629", "3630", "3631", "3632", "3633", "3634", "3635", "3636", "3637", "3638", "3639", "3640", "3641", "3642", "3643", "3644", "3645", "3646", "3647", "3648", "3649", "3650", "3653", "3669", "3670", "3671", "3672", "3676", "3677", "3678", "3679", "3680", "3681", "3682", "3683", "3684", "3685", "3686", "3687", "3688", "3689", "3690", "3691", "3692", "3693", "3694", "3696", "3697", "3698", "3699", "3700", "3702", "3703", "3704", "3705", "3706", "3707", "3708", "3709", "3710", "3711", "3712", "3713", "3714", "3715", "3716", "3717", "3718", "3719", "3720", "3721", "3722", "3742", "3743", "3744", "3745", "3746", "3747", "3748", "3749", "3750", "3751", "3752", "3753", "3754", "3755", "3756", "3757", "3758", "3759", "3760", "3761", "3762", "3763", "3764", "3765", "3766", "3767", "3768", "3769", "3770", "3771", "3772", "3773", "3774", "3775", "3776", "3777", "3778", "3779", "3780", "3781", "3782", "3783", "3784", "3785", "3786", "3787", "3788", "3789", "3790", "3791", "3792", "3793", "3794", "3795", "3796", "3797", "3798", "3799", "3800", "3801", "3802", "3803", "3804", "3805", "3806", "3807", "3808", "3809", "3810", "3811", "3812", "3813", "3814", "3815", "3816", "3817", "3818", "3819", "3820", "3821", "3822", "3823", "3824", "3825", "3826", "3827", "3828", "3829", "3830", "3831", "3832", "3833", "3834", "3835", "3836", "3837", "3838", "3839", "3840", "3841", "3842", "3843", "3844", "3845", "3846", "3847", "3848", "3849", "3850", "3851", "3852", "3853", "3854", "3855", "3856", "3857", "3858", "3859", "3860", "3861", "3862", "3863", "3864", "3865", "3866", "3867", "3868", "3869", "3870", "3871", "3872", "3873", "3876", "3877", "3878", "3879", "3880", "3881", "3882", "3883", "3884", "3885", "3895", "3896", "3897", "3898", "3899", "3900", "3901", "3902", "3903", "3904", "3905", "3908", "3909", "3910", "3911", "3912", "3913", "3920", "3921", "3922", "3923", "3925", "3926", "3927", "3928", "3929", "3930", "3931", "3933", "3934", "3935", "3936", "3937", "3944", "3946", "3947", "3948", "3949", "3950", "3951", "3952", "3953", "3954", "3955", "3956", "3957", "3958", "3963", "3964", "3975", "3976", "3977", "3978", "3979", "3980", "3981", "3982", "3983", "3984", "3985", "3986", "3987", "3988", "3989", "3990", "3994", "4001", "4005", "4007", "4008", "4011", "4012", "4013", "4016", "4017", "4018", "4020", "4021", "4024", "4025", "4026", "4027", "4029", "4030", "4034", "4038", "4039", "4041", "4043", "4045", "4047", "4048", "4049", "4052", "4053", "4054", "4057", "4058", "4059", "4062", "4063", "4064", "4067", "4070", "4074", "4078", "4080", "4083", "4086", "4087", "4089", "4090", "4094", "4095", "4096", "4097", "4098", "4101", "4105", "4106", "4109", "4110", "4112", "4113", "4116", "4117", "4119", "4121", "4122", "4126", "4127", "4130", "4133", "4134", "4137", "4138", "4141", "4142", "4148", "4149", "4150", "4151", "4153", "4154", "4156", "4158", "4160", "4161", "4162", "4163", "4164", "4165", "4166", "4167", "4168", "4169", "4170", "4171", "4173", "4176", "4179", "4182", "4183", "4185", "4186", "4191", "4194", "4197", "4198", "4200", "4203", "4206", "4209", "4212", "4215", "4216", "4218", "4219", "4221", "4222", "4224", "4225", "4227", "4230", "4233", "4236", "4240", "4241", "4247", "4248", "4249", "4250", "4251", "4255", "4262", "4266", "4268", "4269", "4272", "4273", "4274", "4277", "4278", "4279", "4281", "4282", "4285", "4286", "4287", "4288", "4290", "4291", "4295", "4301", "4304", "4305", "4310", "4311", "4312", "4318", "4321", "4324", "4327", "4330", "4333", "4336", "4339", "4342", "4345", "4348", "4349", "4351", "4354", "4357", "4360", "4361", "4363", "4366", "4369", "4372", "4375", "4379", "4382", "4385", "4388", "4389", "4391", "4392", "4411", "4412", "4413", "4414", "4415", "4416", "4417", "4418", "4419", "4420", "4421", "4422", "4423", "4424", "4425", "4426", "4427", "4428", "4429", "4430", "4431", "4432", "4433", "4434", "4435", "4436", "4437", "4438", "4439", "4440", "4441", "4442", "4443", "4444", "4457", "4458", "4459", "4460", "4461", "4462", "4463", "4464", "4465", "4466", "4467", "4468", "4469", "4470", "4471", "4472", "4473", "4474", "4475", "4476", "4477", "4478", "4479", "4480", "4481", "4482", "4483", "4484", "4485", "4486", "4487", "4488", "4489", "4490", "4491", "4492", "4493", "4494", "4495", "4496", "4497", "4498", "4499", "4500", "4501", "4502", "4597", "4598", "4599", "4600", "4601", "4602", "4603", "4604", "4605", "4606", "4607", "4608", "4609", "4610", "4611", "4612", "4613", "4614", "4615", "4616", "4617", "4618", "4619", "4620", "4625", "4626", "4627", "4628", "4633", "4634", "4635", "4636", "4637", "4638", "4639", "4640", "4641", "4642", "4643", "4644", "4645", "4646", "4647", "4648", "4649", "4650", "4651", "4652", "4653", "4654", "4655", "4680", "4681", "4682", "4683", "4684", "4685", "4686", "4687", "4688", "4689", "4690", "4691", "4692", "4693", "4694", "4695", "4696", "4697", "4698", "4699", "4700", "4701", "4738", "4739", "4740", "4741", "4742", "4743", "4744", "4747", "4815", "4816", "4823", "4824", "4825", "4826", "4830", "4833", "4848", "4849", "4850", "4851", "4873", "4881", "4882", "4883", "4884", "4885", "4886", "4887", "4888", "4889", "4890", "4891", "4892", "4893", "4894", "4895", "4896", "4897", "4898", "4899", "4900", "4901", "4902", "4903", "4904", "4905", "4906", "4907", "4908", "4909", "4910", "4911", "4912", "4913", "4914", "4915", "4916", "4917", "4918", "4919", "4920", "4921", "4922", "4923", "4924", "4925", "4926", "4927", "4928", "4929", "4930", "4931", "4932", "4933", "4934", "4935", "4936", "4937", "4938", "4939", "4940", "4941", "4942", "4943", "4944", "4945", "4946", "4947", "4948", "4949", "4950", "4951", "4952", "4953", "4954", "4955", "4956", "4957", "4958", "4959", "4960", "4961", "4962", "4963", "4964", "4965", "4966", "4967", "4968", "4969", "4970", "4972", "4973", "4974", "4975", "4976", "4977", "4978", "4982", "4983", "4987", "4988", "4989", "4990", "4994", "4995", "4996", "4997", "4999", "5001", "5002", "5003", "5004", "5005", "5006", "5007", "5008", "5009", "5010", "5011", "5012", "5022", "5023", "5024", "5025", "5026", "5027", "5028", "5029", "5030", "5031", "5032", "5033", "5034", "5035", "5036", "5037", "5038", "5039", "5040", "5041", "5042", "5043", "5044", "5046", "5055", "5056", "5071", "5072", "5073", "5074", "5075", "5076", "5077", "5078", "5079", "5081", "5082", "5083", "5084", "5085", "5086", "5087", "5088" };

        // Current sector file name
        public static string current_file = "";

        static void Main(string[] args)
        {
            if (File.Exists("monster.db") && Directory.Exists("origmap"))
            {
                Console.WriteLine("Extracting and converting 'monster.db' to XML format.");
                Console.WriteLine($"Total Monster Spawns: {total_spawns}");

                // BEGIN
                Console.WriteLine($"Getting sector file names...");
                foreach (string line in File.ReadAllLines("monster.db").Skip(5))
                {
                    if (line.Contains("# ======"))
                    {
                        // Get current sector file name
                        current_file = line.Split(new char[] { '#', ' ', '=' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault().Replace(",","-") + ".sec";
                        Console.WriteLine(Environment.NewLine + $"File: {current_file}");
                    }
                    else if (current_file != "" && !line.Contains("#"))
                    {
                        // Begin generating the monster spawns
                        string[] values = line.Split(new char[] { '#', ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                        Spawn spawn = new Spawn();
                        spawn.SectorFile = current_file;
                        spawn.Amount = Int32.Parse(values[5]);
                        spawn.Radius = Int32.Parse(values[4]);
                        spawn.Regen = Int32.Parse(values[6]);
                        spawn.StartingPointX = Int32.Parse(values[1], NumberStyles.Any);
                        spawn.StartingPointY = Int32.Parse(values[2], NumberStyles.Any);
                        spawn.StartingPointZ = Int32.Parse(values[3], NumberStyles.Any);

                        // Monster id and name
                        spawn.MonsterId = Int32.Parse(values[0]);
                        spawn.MonsterName = MonsterName(spawn.MonsterId);

                        // Add sppawn to list
                        spawns.Add(spawn);
                        Console.WriteLine($"Amount: {spawn.Amount} - Radius: {spawn.Radius} - Regen: {spawn.Regen} - Start: [{spawn.StartingPointX}, {spawn.StartingPointY}, {spawn.StartingPointZ}] - Monster ID: {spawn.MonsterId} - Monster Name: {spawn.MonsterName}");
                    }
                }

                // Begin writing the XML file
                output = "<?xml version=\"1.0\"?>" + Environment.NewLine;
                output += "<spawns>" + Environment.NewLine;

                // Iterate through each spawn
                foreach (Spawn spawn in spawns)
                {
                    // Add single spawns directly
                    if (spawn.Amount == 1)
                    {
                        output += $"\t<spawn centerx=\"{spawn.StartingPointX}\" centery=\"{spawn.StartingPointY}\" centerz=\"{spawn.StartingPointZ}\" radius=\"{spawn.Radius}\">" + Environment.NewLine;
                        output += $"\t\t<monster name=\"{spawn.MonsterName}\" x=\"0\" y=\"0\" z=\"{spawn.StartingPointZ}\" spawntime=\"60\"/>" + Environment.NewLine;
                        output += "\t</spawn>" + Environment.NewLine;

                        coordinates.Add(new Coordinate { PositionX = spawn.StartingPointX, PositionY = spawn.StartingPointY, PositionZ = spawn.StartingPointZ });
                        Console.WriteLine($"Added single spawn: \t[{spawn.StartingPointX}, {spawn.StartingPointY}, {spawn.StartingPointZ}]\t<monster name=\"{spawn.MonsterName}\" x=\"0\" y=\"0\" z=\"{spawn.StartingPointZ}\" spawntime=\"60\"/>");
                    }
                    else if (spawn.Amount > 1)
                    {
                        // Add multiple spawns
                        output += $"\t<spawn centerx=\"{spawn.StartingPointX}\" centery=\"{spawn.StartingPointY}\" centerz=\"{spawn.StartingPointZ}\" radius=\"{spawn.Radius}\">" + Environment.NewLine;

                        // Iterate through all monsters
                        for (int i = 0; i < spawn.Amount; i++)
                        {
                            // Calculate the position using offsets
                            spawn.OffsetPointX = CalculateOffset(new Coordinate { PositionX = spawn.StartingPointX, PositionY = spawn.StartingPointY, PositionZ = spawn.StartingPointZ }, i, spawn.SectorFile)[0];
                            spawn.OffsetPointY = CalculateOffset(new Coordinate { PositionX = spawn.StartingPointX, PositionY = spawn.StartingPointY, PositionZ = spawn.StartingPointZ }, i, spawn.SectorFile)[1];

                            // Write the output
                            output += $"\t\t<monster name=\"{spawn.MonsterName}\" x=\"{spawn.OffsetPointX}\" y=\"{spawn.OffsetPointY}\" z=\"{spawn.StartingPointZ}\" spawntime=\"60\"/>" + Environment.NewLine;
                            
                            Console.WriteLine($"Added multi spawn: \tStart: [{spawn.StartingPointX}, {spawn.StartingPointY}, {spawn.StartingPointZ}] -- Monster Spawn: [{spawn.StartingPointX + spawn.OffsetPointX}, {spawn.StartingPointY + spawn.OffsetPointY}, {spawn.StartingPointZ}]\t<monster name=\"{spawn.MonsterName}\" x=\"{spawn.OffsetPointX}\" y=\"{spawn.OffsetPointY}\" z=\"{spawn.StartingPointZ}\" spawntime=\"60\"/>");
                        }

                        // End the spawn segment
                        output += "\t</spawn>" + Environment.NewLine;
                    }
                }

                // End the file and write it out
                output += "</spawns>" + Environment.NewLine;
                File.WriteAllText("output.xml", output);
            }
            else
            {
                Console.WriteLine("Please place the 'monster.db' file and the entire 'origmap' folder (with the sector files) in this directory.");
            }
        }

        /// <summary>
        /// Function that will calculate the amount of spawns in the monster.db file
        /// </summary>
        /// <returns>Amount of monster spawns</returns>
        static int AmountOfSpawns()
        {
            // Iterate through the lines
            string[] values;
            int totalAmount = 0;
            foreach (string line in File.ReadLines("monster.db").Skip(5))
            {
                // Exclude commented lines
                if (!line.Contains("#"))
                {
                    // Get the "Amount"-segment value
                    values = line.Split(new char[] { '#', ' ', '=' }, StringSplitOptions.RemoveEmptyEntries);
                    totalAmount += Int32.Parse(values[5]);
                }
            }

            // Return the amount of monster spawns
            return totalAmount;
        }

        /// <summary>
        /// Function that will convert monster id number to name
        /// </summary>
        /// <returns>Monster name</returns>
        static string MonsterName(int id)
        {
            string monster_name;
            switch (id)
            {
                case 77: monster_name = "Amazon"; break;
                case 79: monster_name = "Ancient Scarab"; break;
                case 234: monster_name = "Apocalypse"; break;
                case 224: monster_name = "Assassin"; break;
                case 105: monster_name = "Badger"; break;
                case 223: monster_name = "Bandit"; break;
                case 78: monster_name = "Banshee"; break;
                case 122: monster_name = "Bat"; break;
                case 230: monster_name = "Bazir"; break;
                case 16: monster_name = "Bear"; break;
                case 55: monster_name = "Behemoth"; break;
                case 17: monster_name = "Beholder"; break;
                case 46: monster_name = "Black Knight"; break;
                case 13: monster_name = "Black Sheep"; break;
                case 80: monster_name = "Blue Djinn"; break;
                case 101: monster_name = "Bonebeast"; break;
                case 45: monster_name = "Bug"; break;
                case 227: monster_name = "Butterfly"; break;
                case 213: monster_name = "Butterfly"; break;
                case 228: monster_name = "Butterfly"; break;
                case 235: monster_name = "Butterfly"; break;
                case 120: monster_name = "Carniphilia"; break;
                case 56: monster_name = "Cave Rat"; break;
                case 124: monster_name = "Centipede"; break;
                case 111: monster_name = "Chicken"; break;
                case 81: monster_name = "Cobra"; break;
                case 112: monster_name = "Crab"; break;
                case 119: monster_name = "Crocodile"; break;
                case 100: monster_name = "Crypt Shambler"; break;
                case 22: monster_name = "Cyclops"; break;
                case 225: monster_name = "Dark Monk"; break;
                case 102: monster_name = "Deathslicer"; break;
                case 31: monster_name = "Deer"; break;
                case 204: monster_name = "Demodras"; break;
                case 35: monster_name = "Demon"; break;
                case 37: monster_name = "Demon Skeleton"; break;
                case 203: monster_name = "Dharalion"; break;
                case 32: monster_name = "Dog"; break;
                case 34: monster_name = "Dragon"; break;
                case 39: monster_name = "Dragon Lord"; break;
                case 69: monster_name = "Dwarf"; break;
                case 66: monster_name = "Dwarf Geomancer"; break;
                case 70: monster_name = "Dwarf Guard"; break;
                case 71: monster_name = "Dwarf Soldier"; break;
                case 215: monster_name = "Dworc Fleshhunter"; break;
                case 216: monster_name = "Dworc Venomsniper"; break;
                case 214: monster_name = "Dworc Voodoomaster"; break;
                case 103: monster_name = "Efreet"; break;
                case 108: monster_name = "Elder Beholder"; break;
                case 211: monster_name = "Elephant"; break;
                case 62: monster_name = "Elf"; break;
                case 63: monster_name = "Elf Arcanist"; break;
                case 64: monster_name = "Elf Scout"; break;
                case 210: monster_name = "The Evil Eye"; break;
                case 206: monster_name = "Fernfang"; break;
                case 231: monster_name = "Ferumbras"; break;
                case 40: monster_name = "Fire Devil"; break;
                case 49: monster_name = "Fire Elemental"; break;
                case 93: monster_name = "Flamethrower"; break;
                case 212: monster_name = "Flamingo"; break;
                case 53: monster_name = "Frost Troll"; break;
                case 75: monster_name = "Gamemaster"; break;
                case 95: monster_name = "Gargoyle"; break;
                case 109: monster_name = "Gazer"; break;
                case 48: monster_name = "Ghost"; break;
                case 18: monster_name = "Ghoul"; break;
                case 38: monster_name = "Giant Spider"; break;
                case 61: monster_name = "Goblin"; break;
                case 51: monster_name = "Green Djinn"; break;
                case 205: monster_name = "Grorlam"; break;
                case 232: monster_name = "The Halloween Hare"; break;
                case 73: monster_name = "Hero"; break;
                case 202: monster_name = "The Horned Fox"; break;
                case 1: monster_name = "Human"; break;
                case 11: monster_name = "Hunter"; break;
                case 94: monster_name = "Hyaena"; break;
                case 121: monster_name = "Hydra"; break;
                case 107: monster_name = "Demon (illusion)"; break;
                case 233: monster_name = "Infernatil"; break;
                case 116: monster_name = "Kongra"; break;
                case 82: monster_name = "Larva"; break;
                case 99: monster_name = "Lich"; break;
                case 41: monster_name = "Lion"; break;
                case 114: monster_name = "Lizard Sentinel"; break;
                case 115: monster_name = "Lizard Snakecharmer"; break;
                case 113: monster_name = "Lizard Templar"; break;
                case 98: monster_name = "Magicthrower"; break;
                case 104: monster_name = "Marid"; break;
                case 117: monster_name = "Merlkin"; break;
                case 92: monster_name = "Mimic"; break;
                case 25: monster_name = "Minotaur"; break;
                case 24: monster_name = "Minotaur Archer"; break;
                case 29: monster_name = "Minotaur Guard"; break;
                case 23: monster_name = "Minotaur Mage"; break;
                case 57: monster_name = "Monk"; break;
                case 229: monster_name = "Morgaroth"; break;
                case 65: monster_name = "Mummy"; break;
                case 207: monster_name = "General Murius"; break;
                case 9: monster_name = "Necromancer"; break;
                case 209: monster_name = "Necropharus"; break;
                case 208: monster_name = "The Old Widow"; break;
                case 5: monster_name = "Orc"; break;
                case 8: monster_name = "Orc Berserker"; break;
                case 59: monster_name = "Orc Leader"; break;
                case 4: monster_name = "Orc Rider"; break;
                case 6: monster_name = "Orc Shaman"; break;
                case 50: monster_name = "Orc Spearman"; break;
                case 2: monster_name = "Orc Warlord"; break;
                case 7: monster_name = "Orc Warrior"; break;
                case 201: monster_name = "Orshabaal"; break;
                case 123: monster_name = "Panda"; break;
                case 217: monster_name = "Parrot"; break;
                case 91: monster_name = "Ashmunrah"; break;
                case 87: monster_name = "Dipthrah"; break;
                case 86: monster_name = "Mahrdis"; break;
                case 84: monster_name = "Morguthis"; break;
                case 90: monster_name = "Omruc"; break;
                case 88: monster_name = "Rahemos"; break;
                case 89: monster_name = "Thalas"; break;
                case 85: monster_name = "Vashresamun"; break;
                case 60: monster_name = "Pig"; break;
                case 96: monster_name = "Plaguethrower"; break;
                case 36: monster_name = "Poison Spider"; break;
                case 42: monster_name = "Polar Bear"; break;
                case 58: monster_name = "Priestess"; break;
                case 74: monster_name = "Rabbit"; break;
                case 21: monster_name = "Rat"; break;
                case 26: monster_name = "Rotworm"; break;
                case 83: monster_name = "Scarab"; break;
                case 43: monster_name = "Scorpion"; break;
                case 220: monster_name = "Serpent Spawn"; break;
                case 14: monster_name = "Sheep"; break;
                case 97: monster_name = "Shredderthrower"; break;
                case 118: monster_name = "Sibang"; break;
                case 33: monster_name = "Skeleton"; break;
                case 106: monster_name = "Skunk"; break;
                case 19: monster_name = "Slime"; break;
                case 20: monster_name = "Slime"; break;
                case 222: monster_name = "Smuggler"; break;
                case 28: monster_name = "Snake"; break;
                case 30: monster_name = "Spider"; break;
                case 221: monster_name = "Spit Nettle"; break;
                case 72: monster_name = "Stalker"; break;
                case 67: monster_name = "Stone Golem"; break;
                case 76: monster_name = "Swamp Troll"; break;
                case 219: monster_name = "Tarantula"; break;
                case 218: monster_name = "Terror Bird"; break;
                case 125: monster_name = "Tiger"; break;
                case 15: monster_name = "Troll"; break;
                case 12: monster_name = "Valkyrie"; break;
                case 68: monster_name = "Vampire"; break;
                case 10: monster_name = "Warlock"; break;
                case 3: monster_name = "War Wolf"; break;
                case 44: monster_name = "Wasp"; break;
                case 47: monster_name = "Wild Warrior"; break;
                case 52: monster_name = "Winter Wolf"; break;
                case 54: monster_name = "Witch"; break;
                case 27: monster_name = "Wolf"; break;
                case 110: monster_name = "Yeti"; break;
                default: monster_name = "UNKNOWN_" + id; break;
            }

            return monster_name;
        }

        /// <summary>
        /// Function that will generate the coordinate where the monster will spawn
        /// </summary>
        /// <returns>A valid monster spawn coordinate</returns>
        static List<int> CalculateOffset(Coordinate coordinate, int index, string sector_file)
        {
            // Does tile include non-walkable item?
            bool isBlocked = false;

            if (index == offset_x.Count)
                index = 0;

            // Spawn offsets
            int offX = offset_x[index];
            int offY = offset_y[index];

            // Calculate difference between spawn (coordinate) and the starting point (in the file)
            string file_name = sector_file.Replace(".sec", "");
            int fileStartX = Int32.Parse(file_name.Split('-').FirstOrDefault(), NumberStyles.Any) * 32;
            int fileStartY = Int32.Parse(file_name.Split('-').Skip(1).FirstOrDefault(), NumberStyles.Any) * 32;
            int diffX = coordinate.PositionX - fileStartX;
            int diffY = coordinate.PositionY - fileStartY;

            // Some files include leading zero's, while some don't
            string str_spawn_offset_x = diffX < 10 ? "0" + diffX.ToString() : diffX.ToString();
            string str_spawn_offset_y = diffY < 10 ? "0" + diffY.ToString() : diffY.ToString();
            // Iterate through the sector file
            foreach (string line in File.ReadAllLines("origmap/" + sector_file))
            {
                // Find the correct line, using the offset
                if (line.StartsWith($"{diffX}-{diffY}:") || line.StartsWith($"{str_spawn_offset_x}-{str_spawn_offset_y}:"))
                {
                    // Check if this coordinate contains an invalid item
                    foreach (string id in invalid_items)
                    {
                        if (Regex.IsMatch(line, @"\b" + id + @"\b") == true)
                        {
                            // Try again
                            isBlocked = true;
                        }
                    }
                }

                if (isBlocked == false)
                {
                    // Return the coordinate if it doesn't already exist
                    bool alreadyExists = coordinates.Any(item => item.PositionX == coordinate.PositionX + offX && item.PositionY == coordinate.PositionY + offY && item.PositionZ == coordinate.PositionZ);
                    if (alreadyExists == false)
                    {
                        // Return the monster spawn offset
                        return new List<int> { offX, offY };
                    }
                    else
                    {
                        // Try again with the next tile
                        CalculateOffset(coordinate, index + 1, sector_file);
                    }
                }
                else
                {
                    // Try again with the next tile
                    CalculateOffset(coordinate, index + 1, sector_file);
                }
            }

            // If all else fails for some reason; return X & Y offsets with value 99 (error)
            return new List<int> { 99, 99 };
        }
    }
}
