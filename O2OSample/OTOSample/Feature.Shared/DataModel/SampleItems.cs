using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Data.Json;
using System.Linq;

namespace Feature.DataModel
{
    public class SampleItems
    {
        private List<SampleItem> list = new List<SampleItem>();
        public List<SampleItem> List
        {
            get { return this.list; }
            set { this.list = value; }
        }

        public List<GroupInfoList> Data { get; set; }

        public SampleItems(Channel channel)
        {
            switch (channel)
            { 
                case Channel.movie:
                    this.loadMovieData();
                    break;
                case Channel.food:
                    this.loadFoodData();
                    break;
                case Channel.lottery:
                    this.loadLotteryData();
                    break;
                case Channel.hotel:
                    this.loadHotelData();
                    break;
                case Channel.entertainment:
                    this.loadEntertainmentData();
                    break;
                case Channel.life:
                    this.loadLifeData();
                    break;
                case Channel.beauty:
                    this.loadBeautyData();
                    break;
                case Channel.traveling:
                    this.loadTravelingData();
                    break;
                case Channel.shop:
                    this.loadShopData();
                    break;
                case Channel.gift:
                    this.loadGiftData();
                    break;
                case Channel.business:
                    this.loadBusinessData();
                    break;
                case Channel.hall:
                    this.loadHallData();
                    break;
                case Channel.cinema:
                    this.loadCinemaData();
                    break;
            }
        }

        public SampleItems() 
        {
            this.loadSampleItem();
        }

        public SampleItems(string content)
        {
            try
            {
                if (!string.IsNullOrEmpty(content))
                {
                    JsonObject jObject = JsonObject.Parse(content);
                    if (jObject["List"] != null)
                    {
                        JsonArray jArray = jObject["List"].GetArray();
                        this.List = (from node in jArray
                                     select new SampleItem(node.Stringify()) { }).ToList<SampleItem>();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string Organizate()
        {
            string result = "{";
            if (this.List != null && this.List.Count > 0)
            {
                result += "\"List\":[";
                foreach (SampleItem sampleItem in this.List)
                {
                    result += sampleItem.Organizate() + ",";
                }
                result = result.TrimEnd(',');
                result += "]";
            }
            result += "}";
            return result;
        }

        private void loadSampleItem()
        {
            this.Data = new List<GroupInfoList>();
            foreach (Channel channel in Enum.GetValues(typeof(Channel)))
            {
#if !WINDOWS_PHONE_APP
                if (channel == Channel.business || channel == Channel.hall || channel == Channel.cinema)
                    continue;
#endif
                string title = string.Empty;
                switch (channel)
                {
                    case Channel.movie:
                        this.loadMovieData();
                        title = "电影";
                        break;
                    case Channel.food:
                        this.loadFoodData();
                        title = "美食";
                        break;
                    case Channel.lottery:
                        this.loadLotteryData();
                        title = "彩票";
                        break;
                    case Channel.hotel:
                        this.loadHotelData();
                        title = "酒店";
                        break;
                    case Channel.entertainment:
                        this.loadEntertainmentData();
                        title = "娱乐";
                        break;
                    case Channel.life:
                        this.loadLifeData();
                        title = "生活";
                        break;
                    case Channel.beauty:
                        this.loadBeautyData();
                        title = "丽人";
                        break;
                    case Channel.traveling:
                        this.loadTravelingData();
                        title = "旅游";
                        break;
                    case Channel.shop:
                        this.loadShopData();
                        title = "购物";
                        break;
                    case Channel.gift:
                        this.loadGiftData();
                        title = "礼物";
                        break;
                    case Channel.business:
                        this.loadBusinessData();
                        title = "商业";
                        break;
                    case Channel.hall:
                        this.loadHallData();
                        title = "放映厅";
                        break;
                    case Channel.cinema:
                        this.loadCinemaData();
                        title = "影院";
                        break;
                }
                GroupInfoList data = new GroupInfoList();
#if !WINDOWS_PHONE_APP
                if (this.List.Count > 6)
                    this.List.RemoveRange(6, this.List.Count - 6);
#endif
                List < SampleItem > sampleItem = new List<SampleItem>(this.List);
                data.GroupTitle = title;
                foreach (SampleItem item in sampleItem)
                    item.TitleGroup = title;
                data.ItemContent = sampleItem;
                this.Data.Add(data);
            }
        }
    

        /// <summary>
        /// load movie
        /// </summary>
        private void loadMovieData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/0.jpg", "爱情狂想曲", "丛兿", "剧情、喜剧", "大陆", "96分钟", "2014-11-28", "李威,苏小妹,童苡萱,付曼,秦汉擂,崔文璐,于金龙", "张成功（李威饰），论财，上有土豪大哥，下有留学博士牙医头衔，金光闪闪惹人爱。论貌，西装领结绅士帽，微微一笑很倾城。张成功各方面条件都逼格满满，一回国便成为炙手可热“国民老公”。而他却不愿理会蜂拥而至的各路美女，唯独对“冷面铁血女汉子”韩雪霏（苏小妹饰）情有独钟，甘做围着女王团团转的“小蜜蜂”。随后，胸大无脑三流女演员毛毛（付曼饰），伟哥集团公子哥阿B（崔文璐饰），韩雪霏妹妹韩晓丽（童苡萱饰）也纷纷加入这场爱情大混战中。爱情狂想，想而不做是怂货，做而不想是流氓。在爱情里交出你的自尊与节操，为爱疯狂秀下限！"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/1.jpg", "匆匆那年", "张一白", "爱情、校园、青春", "大陆", "120分钟", "2014-12-05", "彭于晏,倪妮,郑恺,魏晨,张子萱,陈赫", "张一白《将爱》之后再拍青春爱情片，根据九夜茴同名小说改编。电影讲述了阳光少年陈寻（彭于晏饰）、痴心女孩方茴（倪妮饰）、纯情备胎赵烨（郑恺饰）、温情暖男乔燃（魏晨饰）、豪放女神林嘉茉（张子萱饰）这群死党跨越十五年的青春、记忆与友情"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/2.jpg", "花咒", "李克龙", "爱情、恐怖、惊悚", "大陆", "112分钟", "2014-12-12", "廖蔚蔚,杨紫彤,张晓燕,张梦恬,田雨晴,罗彬,汪奇,齐志", "上世纪三十年代，一栋古老而又鬼魅丛生的别墅里。女主人燕贞（张晓燕 饰）不明就里坠楼，引发男主人书明（罗彬 饰）的恍惚癔念，私人侦探上官天（齐志 饰）和中药铺的小研（廖蔚蔚展开 饰）被动介入，试图揭开迷雾，谁料发现书明的私人医生苏岑（张梦恬 饰）风干的尸体，而主人五岁的女儿丝丝，每天都说在和死去的妈妈燕贞荡秋千，古怪而又灵异的行为让这神秘的宅子里飘荡起噬魂的气息，丑陋骇人的花匠老袁（汪奇 饰），也突然变成了一个口舌生疮，不能发声的哑巴，女佣夏荷（杨紫彤 饰）的行为日渐诡异，更是引发侦探的疑窦丛生，越想抽丝剥茧，却发现手脚被一个神出鬼没的鬼影牢牢束缚，险些自身难保，这个鬼影来自何处…… "));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/3.jpg", "黄飞鸿之英雄有梦", "周显扬", "剧情、动作、传记", "香港", "131分钟", "2014-11-21", "彭于晏,王珞丹,梁家辉,井柏然,王祖蓝,洪金宝,杨颖,张晋,冯嘉怡,秦俊杰,高泰宇,文峰", "这是黄飞鸿年轻的故事，他注定成为时代的宗师，在武侠世界中一个永恒的传奇。1868年的清末，朝廷腐败让人民生活备受煎熬。在广州，黑虎帮和北海帮子两大恶势力横行于黄埔港。他们以暴力威胁统治在港口的穷苦人民，让他们生活在水深火热之中。外界在看来，黄埔港是个繁荣并充满机会地方，但深入其中，会发现这里其实是人间炼狱。黑虎帮的老板雷公（洪金宝 饰）因一个新兵手下阿飞（彭于晏 饰），以过人身手及胆识直取北海帮子首领的头脑，而让黑虎帮独霸一方，不但靠鸦片赚进大笔银两，更进行着卑鄙的贩卖人口勾当，雷公更视阿飞如己出。其实，阿飞背后另有故事，心中更另有计划。他和肝胆相照的朋友们设了陷阱，救出穷苦的人们，更公开与雷公进行生死决斗。黄埔港终于恢复了和平，也诞生了真正的英雄，这是黄飞鸿故事的开始……"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/4.jpg", "火线反攻", "大卫·布莱特", "剧情、动作、惊悚", "美国", "95分钟", "2014-11-28", "布鲁斯·威利斯,乔什·杜哈明,罗莎里奥·道森,文森特·多诺费奥", "勇猛果敢的杰里米·科曼（乔什·杜哈明 Josh Duhamel 饰）是一名备受欢迎的消防员，某晚在执行完任务后，他进入便利店购物，却遭遇率领手下抢夺地盘的黑帮头子戴维·黑根（文森特·诺费奥 Vincent D'Onofrio 饰），店长及其家人被杀，杰里米侥幸逃脱。之后他作为证人指认凶手，但对戴维为人极其了解的警官迈克·塞拉（布鲁斯·威利斯 Bruce Willis 饰）劝说杰里米接受证人保护，并隐姓埋名，以防止遭到戴维的无情报复。但是他的行踪很快被戴维一伙发现，他与女友塔莉亚（罗莎里奥·道森 Rosario Dawson 饰）遭到伏击，女友更身受重伤。穷凶极恶的戴维，势要将杰里米逼向死路，困兽犹斗，杰里米展开绝死反抗……"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/5.jpg", "狂怒", "大卫·阿耶", "剧情、动作、战争", "美国", "132分钟", "2014-11-21", "布拉德·皮特,希亚·拉博夫,罗根·勒曼,乔·博恩瑟，斯科特·伊斯特伍德,詹森·艾萨克,迈克尔·佩纳,沙维尔·塞缪尔", "在1945年二战硝烟即将消散之时，同盟国军队准备在欧洲战场发动最后一轮猛烈攻击。在以寡敌众、弹尽粮绝的不利条件下，身经百战的陆军中士Wardaddy（布拉德·皮特饰演）指挥谢尔曼坦克的几名坦克手以及二等兵Norman（希亚·拉博夫饰演）深入纳粹德国的中心地带，执行一项死亡任务。"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/6.jpg", "马达加斯加的企鹅", "埃里克·达尼尔、史密斯", "动画、冒险、喜剧", "美国", "92分钟", "2014-11-14", "汤姆·麦克格雷斯,克里斯·米勒,本尼迪克特·康伯巴奇,安迪·里查克", "作为《马达加斯加》外传，影片《马达加斯加的企鹅》将以人气角色“企鹅四贱客”——Skipper，Kowalski，Rico和Private为主角，讲述他们加入一个秘密间谍组织，与邪恶的“章鱼怪”吹气孔博士展开斗争的故事。四只贱气十足的企鹅在执行任务的过程中笑料百出，欢乐不断，还结识了本尼迪克特·康伯巴奇配音的特工狼带领的北极小分队。"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/7.jpg", "密道追踪", "俞岛", "惊悚、恐怖、冒险", "大陆", "95分钟", "2014-12-05", "唐文龙,母其弥雅,李渊,李海涛,郭常辉,王胜利,郭达,张山", "影片主要讲述了古城镇的钢厂地下惊现金元时期的古墓，千年古墓重见天日，宝藏价值连城，其中的“阴兵虎符”更是无价之宝，各路盗墓人马纷涌而至，夺宝大战一触即发，而虎符背后隐藏的惊天秘密 ，将众人的命运推进千年古墓的生死迷局之中……"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/8.jpg", "名梁海战", "金韩民", "动作、剧情", "韩国", "128分钟", "2014-12-12", "崔岷植,柳承龙,赵震雄,晋久,李贞贤", "故事讲述的是1597年10月26日，朝鲜名将李舜臣在鸣梁海峡以12艘板屋船击退日舰330余艘，创下世界海战史上的一个奇迹。李舜臣改良创新了大型战船“龟船”，甲板坚苦机动灵活，在抗击日军的战斗中发挥了巨大作用。而李舜臣在此次海战中其实已经被国家放弃，要打一场看似没有胜算的战斗，而他却秉持着”必死即生，必生即死“的信念，率领手下的士兵将恐惧化为勇气，以少胜多打了一场胜仗，令人叹为观止。"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/9.jpg", "撒娇女人最好命", "彭浩翔", "喜剧、爱情", "大陆", "95分钟", "2014-11-28", "周迅,黄晓明,隋棠,谢依霖", "张慧（周迅 饰）一直都以女汉子的形态快乐地活着，和好基友兼暗恋对象小恭（黄晓明 饰）一同念大学、一同毕业、一同工作。小恭是个闷骚宅男，因为家境不好、没有经济基础，他坚持在自己事业有成之前保持单身。于是，张慧就一直默默地等待着，谁知没有等来小恭的告白，却等来个程咬金——小恭去台湾出差，遇到一口软糯台湾普通话的撒娇高手蓓蓓（隋棠 饰），一见倾心，坠入情网。张慧猝不及防，不知该如何应对，于是，她身为撒娇女王之首的闺蜜阮美（谢依霖 饰）带领长江以南最厉害的撒娇姐妹团锥子脸姐妹充当军师，为她出谋划策夺回爱人。很快，蓓蓓主动出招，拜托小恭约张慧一起吃饭。在张慧和小恭大学时期常去的大排档中，蓓蓓祭出各种撒娇手段，一顿饭间已完胜盛装出席的张慧。张慧沮丧无比，但阮美并不气馁，决意要将张慧培训称撒娇达人。在阮美等姐妹团的集训下，张慧学习了各种撒娇要点，并开始在交友软件上认识陌生男子……"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/10.jpg", "谁动了我的梦想", "王伟", "剧情", "大陆", "90分钟", "2014-12-11", "张蓝心,立威廉,胡兵,贺彬,王希瑶,苑琼丹,罗家英,郑浩南", "从小到大，董晓瑞（张蓝心 饰）都是个不折不扣的“假小子”，谁说假小子不能有春天？在董晓瑞的心里，青梅竹马的好友王峥（贺彬 饰）正是她深深爱慕的对象。然而，当董晓瑞鼓起勇气向王峥表白之时，换来的却是委婉的拒绝，深感受到伤害的董晓瑞决定远走他乡，前往大都市打拼，以证明自己的价值和能力。一次偶然中，董晓瑞被知名设计师斯蒂文（胡兵 饰）相中成为了首席模特，但之后斯蒂文的离开让董晓瑞重新坠入了水深火热之中，不仅事业受到重创，还处处遭到同事的排挤。之后，董晓瑞成为了腹黑设计师陈默（立威廉 饰）的助理，她的命运会因此而发生改变吗？"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/11.jpg", "太平轮", "吴宇森", "剧情、爱情", "大陆", "120分钟", "2014-12-02", "金城武,黄晓明,章子怡,宋慧乔,张涵予", " 1949年1月27日，在从上海开往台湾基隆途中的上海中联轮船公司轮船“太平轮”号，因超载、夜间航行未开航行灯而被撞沉，导致船上近千名达官显贵、绅士名流及逃亡难民罹难。影片讲述在跨越50多年的历史中，三对不同背景的情侣如何被历史的潮流所影响，经历了战争和灾难，终于找到幸福的故事。"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/12.jpg", "天河", "宁海强,沈东", "剧情", "大陆", "100分钟", "2014-11-15", "李幼斌,俞飞鸿,段奕宏,王若心,高明,赵有亮,林妙可", "南水北调，是一项关系到中华民族福祉的战略性工程。为了此项事关人民生存与发展大计的伟大工程，无数工程建设者和沿线移民用实际行动谱写了一曲感天动地的奉献之歌。董望川（李幼斌 饰）是南水北调中线工程的副总指挥，他与妻子周晓丹（俞飞鸿 饰）为工程忙于各自工作，聚少离多，原本稳固的感情渐渐暴露出问题。在技术攻坚阶段，董望川的学生也是最得力的助手江浩（段奕宏 饰）竟然离他而去，跳槽到一家高薪企业。同时住在库区的亲属坚决反对移民搬迁。种种压力纷至沓来。为了确保工程如期保质完成，董望川亲自坐镇一线，并推荐妻子周晓丹负责移民工作。在最关键的穿黄工程中，大型盾构机突然出现严重故障。当众人一筹莫展之时，江浩挺身而出，化险为夷。北京五棵松地铁站下，一场世界水利工程史上的奇迹被创造。通水日期即将到来，所有人都在为工程的顺利竣工做着最后的努力……"));
            this.list.Add(new SampleItem("ms-appx:///Assets/movie/13.jpg", "我的早更女友", "郭在容", "喜剧、爱情", "大陆", "98分钟", "2014-12-12", "佟大为,周迅,钟汉良,张梓琳", "本以为是身披婚纱的毕业婚礼，却成为惨遭男友拒绝的爱情葬礼，一段刻骨铭心的失败恋情成为26岁戚嘉（周迅 饰）永不逃脱的噩梦：情绪失控、姨妈失调、神经衰弱、状态一团糟……被医生无情宣判为“早更”的她，封闭内心逃避一切时，极品暖男袁晓鸥（佟大为 饰）来到她身边，无微不至的关怀像太阳一般温暖，每天各种欺负与被欺负的“惨剧”啼笑皆非地上演。最终，暖男用坚持不懈的努力，帮早更女重新找回青春和爱情。"));
        }

        /// <summary>
        /// load movie
        /// </summary>
        private void loadCinemaData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("", "万达影城（望京店）", "地址：朝阳区望京街8号", "20", "70", "已售260", "5km"));
            this.list.Add(new SampleItem("", "首都电影院（王府井店）", "地址：东城区王府井大街251号", "31", "65", "已售2460", "5km"));
            this.list.Add(new SampleItem("", "新华国际影城（宝胜店）", "地址：海淀区宝盛北里西区28号", "33", "70", "已售11460", "5km"));
            this.list.Add(new SampleItem("", "金逸影城（望京店）", "地址：朝阳区北苑路42号", "39", "90", "已售6340", "5km"));
            this.list.Add(new SampleItem("", "金棕榈影城（顺义店）", "地址：顺义区新顺南大街8号", "40", "89", "已售15260", "5km"));
            this.list.Add(new SampleItem("", "新安电影城（望京店）", "地址：朝阳区望京街167号", "35", "80", "已售440", "5km"));
            this.list.Add(new SampleItem("", "星聚影城（王府井店）", "地址：东城区王府井大街138号", "40", "90", "已售12543", "5km"));
            this.list.Add(new SampleItem("", "保利国际影城（望京店）", "地址：朝阳区望京新城", "38", "90", "已售6560", "5km"));
            this.list.Add(new SampleItem("", "星星影城（顺义店）", "地址：顺义区新顺南大街118号", "40", "80", "已售4360", "5km"));
            this.list.Add(new SampleItem("", "博威国际影城（昌平店）", "地址：昌平区鼓楼南大街6号", "43", "120", "已售4581", "5km"));
            this.list.Add(new SampleItem("", "丽人影城（望京店）", "地址：西城区白广路8号", "35", "78", "已售5480", "5km"));
            this.list.Add(new SampleItem("", "嘉华电影院（学清路店）", "地址：海淀区学清路甲48号", "35", "89", "已售4185", "5km"));
            this.list.Add(new SampleItem("", "时光影城（朝阳区店）", "地址：朝阳区北三环东路45号", "58", "90", "已售2478", "5km"));
            this.list.Add(new SampleItem("", "嘉和影城（望京店）", "地址：朝阳区望京街48号", "54", "109", "已售12310", "5km"));
            this.list.Add(new SampleItem("", "东都影城（顺义店）", "地址：海淀区成府路28号", "35", "70", "已售23250", "5km"));
            this.list.Add(new SampleItem("", "百顺电影城（望京店）", "地址：朝阳区东大桥路29号", "43", "90", "已售45226", "5km"));
            this.list.Add(new SampleItem("", "禾木影院（中关村店）", "地址：海淀区中关村大街47号", "35", "78", "已售7841", "5km"));
            this.list.Add(new SampleItem("", "华谊影城（奥体店）", "地址：朝阳区湖景东路12号", "45", "120", "已售23460", "5km"));
            this.list.Add(new SampleItem("", "沃美影城（回龙观店）", "地址：昌平区回龙观同成街18号", "34", "79", "已售2300", "5km"));
            this.list.Add(new SampleItem("", "时光美影城（望京店）", "地址：朝阳区广顺北大街19号", "50", "100", "已售7852", "5km")); 
        }

        /// <summary>
        /// load hall
        /// </summary>
        private void loadHallData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem(DateTime.Now.AddMinutes(1).ToString("HH:mm"), DateTime.Now.AddMinutes(90).ToString("HH:mm") + " 结束", "国语3D", "3号厅", "18", "70"));
            this.list.Add(new SampleItem("16:05", "18:04 结束", "国语2D", "5号厅", "23", "80"));
            this.list.Add(new SampleItem("18:55", "20:54 结束", "国语2D", "3号厅", "20", "50"));
            this.list.Add(new SampleItem("21:05", "23:04 结束", "国语3D", "1号厅", "35", "89"));
            this.list.Add(new SampleItem("10:20", "12:19 结束", "国语2D", "3号厅", "28", "78"));
            this.list.Add(new SampleItem("15:25", "17:24 结束", "国语3D", "2号厅", "25", "75"));
            this.list.Add(new SampleItem("19:30", "21:29 结束", "国语2D", "5号厅", "34", "79"));
            this.list.Add(new SampleItem("13:10", "15:04 结束", "英语2D", "4号厅", "43", "90"));
            this.list.Add(new SampleItem("18:25", "20:34 结束", "国语3D", "8号厅", "40", "80"));
            this.list.Add(new SampleItem("16:55", "19:07 结束", "国语2D", "10号厅", "48", "90"));
            this.list.Add(new SampleItem("18:25", "20:37 结束", "国语3D", "12号厅", "38", "110"));
            this.list.Add(new SampleItem("21:05", "23:07 结束", "国语2D", "14号厅", "25", "68"));
        }

        /// <summary>
        /// entertainment Data
        /// </summary>
        private void loadEntertainmentData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/0.jpg", "麦乐迪", "【广外大街】周日至周四黄金时段3小时欢唱券", "288", "1166", "已售7840", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/1.jpg", "KTV", "【多商区】自助餐多时段欢唱券+可连续欢唱+免费WiFi+免费停车位", "48", "250", "已售460", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/2.jpg", "汗蒸馆", "【望京】汗蒸+足底按摩+拔罐套餐", "119", "248", "已售655", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/3.jpg", "美容养生会所", "【亚运村】红花足道/中式按摩二选一+提供免费wifi+免费停车", "560", "1170", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/4.jpg", "儿童主题乐园", "【多商区】超值畅享亲子套票一张+贴心服务+温馨环境+与众不同的游乐体验", "140", "200", "已售523", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/5.jpg", "美奇儿童乐园", "【草桥】儿童乐园畅玩+通票三小时", "35", "77", "已售3800", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/6.jpg", "游泳馆", "【顺义】专业游泳运动场+环境干净整洁", "70", "240", "已售2140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/7.jpg", "温泉", "【房山区】温泉门票1张+男女通用+免费停车位", "98", "168", "已售1430", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/8.jpg", "静之湖滑雪场", "【多商区】周末全天滑雪票：含滑雪用具一套+租用费及魔毯+拖牵和雪道使用费", "68", "178", "已售655", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/9.jpg", "云佛山滑雪场", "【密云县】周末4小时滑雪票+安全有保障", "89", "360", "已售320", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/10.jpg", "健身中心", "【十里堡】健身三小时+免费WiFi", "58", "100", "已售520", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/11.jpg", "瑜伽馆", "【多商区】瑜伽+经络调理至尊卡+释放压力+自由呼吸", "80", "800", "已售74", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/12.jpg", "大堂吧", "【多商区】冬季双人饮酒套餐+不限时段通用", "100", "158", "已售260", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/13.jpg", "船舱酒吧", "【什刹海】啤酒套餐+不限时段通用", "189", "638", "已售156", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/14.jpg", "夜时尚台球", "【新街口】两小时台球畅打+免费停车位", "24", "50", "800", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/15.jpg", "电玩城", "【西单】100个游戏币套餐+免费WiFi+益智健康动感设施+各类经典电玩设备", "51", "100", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/16.jpg", "真人CS", "【昌平区】真人cs激战3小时+免费WiFi+免费停车位", "60", "150", "已售540", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/17.jpg", "合鼎电子", "【多商区】游戏币体验套餐+免费Wifi", "72", "350", "已售980", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/18.jpg", "吧友网咖", "【朝阳区】环境舒适+3小时上网体验", "50", "96", "已售2090", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/19.jpg", "金库KTV", "【西单】周一至周五（11:30-19:00 ）+小房+2张券 ", "29", "180", "已售1200", "5km"));
        }

        /// <summary>
        /// beauty data
        /// </summary>
        private void loadBeautyData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/0.jpg", "美甲沙龙", "【多商区】完美甲油胶套餐+免费WiFi+男女通用", "59", "278", "已售124", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/1.jpg", "爱空间美甲", "【多商区】精致甲油胶美甲套餐+免费WiFi", "79", "100", "已售360", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/2.jpg", "格蕾丝美睫", "【望京】高品质貂绒睫毛嫁接爆浓款+免费WiFi", "268", "1180", "已售355", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/3.jpg", "YOYO美甲", "【西红门】甲油尊享套餐+免费WiFi+免费停车位", "88", "550", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/4.jpg", "甲美乐", "【多商区】美国LKT甲油胶套餐+另送护手霜按摩", "58", "480", "已售263", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/5.jpg", "Lily nails", "【多商区】OPI精护理炫彩套餐+免费WiFi", "60", "120", "已售3800", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/6.jpg", "美发中心", "【顺义】修塑烫染套餐+免费WiFi+长短发不限", "170", "320", "已售2140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/7.jpg", "发型沙龙", "【东直门】菲灵质感创意染发+欧莱雅美奇丝护理", "196", "1040", "已售1430", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/8.jpg", "田园美发", "【中关村】造型师剪发+凯越炫彩染发+韩国阿丽德LPP发膜+3D造型", "220", "430", "已售10170", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/9.jpg", "发型设计", "【多商区】高级剪发师剪发+免费WiFi+男女通用", "68", "238", "已售3200", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/10.jpg", "美发", "【苏州桥】金牌烫发套餐+免费WiFi+男女通用", "258", "1448", "已售2000", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/11.jpg", "美甲", "【多商区】精致甲油胶套餐+免费WiFi", "88", "260", "已售304", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/12.jpg", "MJ造型", "【多商区】烫/染套餐2选1+免费WiFi", "358", "1420", "已售260", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/13.jpg", "美容美发", "【左家庄】高级修塑烫染套餐+免费WiFi+长短发不限", "46", "120", "已售156", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/14.jpg", "婚纱摄影", "【多商区】情侣写真套系：三套服装+外景拍摄", "1800", "5600", "460", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/15.jpg", "美容中心", "【西红门】透明质酸骨胶原面护+颈护套餐+免费WiFi+含淋浴", "188", "750", "已售740", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/16.jpg", "I SPA", "【多商区】男女通用+含淋浴", "100", "200", "已售1540", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/17.jpg", "美丽田园", "【多商区】美白水润全面嫩肤套餐+女士专享", "158", "1060", "已售980", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/18.jpg", "专业美容", "【多商区】科丽妍活细胞水嫩补水护理+拉宝利营养发膜护理套餐+含淋浴", "188", "456", "已售1090", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/19.jpg", "美容养生会所", "【十里堡】美白/保湿/嫩肤/紧致SPA套餐四选一+21年高端专业护肤经验", "320", "780", "已售120", "5km"));
        }

        /// <summary>
        /// traveling Data
        /// </summary>
        private void loadTravelingData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/0.jpg", "划船", "【多商区】划木船体验3小时+接触自然风光", "78", "150", "已售784", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/1.jpg", "顺景温泉", "【多商区】温泉门票1张+男女通用+免费停车位", "278", "336", "已售1100", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/2.jpg", "北戴河一日游", "【望京】海边浴场+感受大海的独特魅力", "320", "450", "已售680", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/3.jpg", "北京一日游", "【亚运村】八达岭长城+往返车票", "160", "198", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/4.jpg", "香山一日游", "【多商区】香山门票+观光枫叶", "150", "324", "已售240", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/5.jpg", "北京古巷一日游", "【草桥】古巷参观游览+导游解说", "60", "130", "已售380", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/6.jpg", "温泉山庄", "【顺义】温泉门票1张+男女通用+免费停车位", "880", "1600", "已售70", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/7.jpg", "军都温泉", "【东直门】温泉假日+含餐票一张", "568", "1306", "已售143", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/8.jpg", "野生动物园", "【中关村】门票一张+观光车游览+导游解说", "69", "80", "已售170", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/9.jpg", "上海东方明珠观光票", "【多商区】观光票+导游解说", "135", "160", "已售1060", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/10.jpg", "冲浪", "【苏州桥】冲浪体验三小时+教练指导", "60", "89", "已售200", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/11.jpg", "颐和园一日游", "【多商区】门票+导游解说+体验自然风光", "135", "248", "已售3400", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/12.jpg", "北京两日游", "【多商区】故宫，天坛，万寿寺等景点", "498", "698", "已售260", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/13.jpg", "南京钟楼观光游", "【左家庄】登钟楼+览风光+导游解说", "70", "140", "已售156", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/14.jpg", "苏州园林", "【六里桥】拙政园门票+导游解说+一览别致的园林建筑", "78", "129", "500", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/15.jpg", "黄山一日游", "【西红门】门票+索道观光", "128", "342", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/16.jpg", "杭州一日游", "【多商区】水乡+船票往返观光", "260", "460", "已售540", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/17.jpg", "马尔代夫6日游", "【多商区】6天4晚+清澈海水+细腻沙粒+寻找世外桃源", "12980", "26880", "已售80", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/18.jpg", "瀑布观光游", "【多商区】门票+导游解说+领悟自然风景", "120", "235", "已售209", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/traveling/19.jpg", "祖山一日游", "【十里堡】门票+有山有水好风光", "68", "140", "已售1200", "5km"));
        }

        /// <summary>
        /// shop data
        /// </summary>
        private void loadShopData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/0.jpg", "圆形衣架", "【广外大街】多头晾晒夹+不锈钢", "28", "50", "已售784", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/1.jpg", "短款棉服女士", "【多商区】短款棉服+保暖舒适随身", "215", "450", "已售760", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/2.jpg", "打底衫", "【望京】红色+蕾丝+毛衣", "89", "138", "已售655", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/3.jpg", "柚子", "【亚运村】2斤+包邮", "10", "17", "已售1400", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/4.jpg", "儿童冬季中长款棉袄", "【多商区】棉服+带领子", "280", "480", "已售223", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/5.jpg", "黑蜜枣", "【草桥】300g+香甜可口", "15", "47", "已售380", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/6.jpg", "皮带", "【顺义】真皮+耐拉+经久实用", "70", "320", "已售2140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/7.jpg", "男士羊毛衫", "【东直门】蓝黄相间+时尚+纯羊毛", "126", "240", "已售143", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/8.jpg", "摄像头", "【中关村】高清晰+防抖+耐用", "280", "430", "已售170", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/9.jpg", "吊坠", "【多商区】情侣吊坠+玉佩", "45", "120", "已售320", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/10.jpg", "帆布鞋", "【苏州桥】厚底+舒适帆布鞋", "158", "298", "已售20", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/11.jpg", "茶具", "【多商区】精致茶具一套+材质安全", "380", "798", "已售34", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/12.jpg", "雪地靴", "【多商区】皮毛一体+保暖+舒适", "128", "269", "已售260", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/13.jpg", "多功能衣架", "【左家庄】塑料+精美晒衣架+多功能", "25", "78", "已售1560", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/14.jpg", "冬季针织帽子", "【六里桥】针织保暖+可爱+白色纯净", "45", "78", "50", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/15.jpg", "男士钱夹", "【西红门】真皮+便携钱夹", "120", "342", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/16.jpg", "沙发垫", "【多商区】花纹+静白+厚底舒适沙发垫", "278", "460", "已售54", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/17.jpg", "双人床四件套", "【多商区】床单+被罩+枕套两个", "148", "950", "已售10", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/18.jpg", "坐垫", "【多商区】舒适保暖+多种颜色可供选择", "15", "56", "已售209", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/shop/19.jpg", "音响", "【十里堡】精美小礼品+音质清晰", "45", "89", "已售1200", "5km"));
        }

        /// <summary>
        /// lottery data
        /// </summary>
        private void loadLotteryData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/lottery/0.jpg", "三星GALAXY Tab S免费送", "【望京】美梦成真+三星GALAXY Tab S+美团网免费送", "0", "0", "已售143600", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/lottery/1.jpg", "三星GALAXY S5免费送", "【多商区】美梦成真+三星GALAXY S5+美团网免费送", "0", "0", "已售183000", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/lottery/2.jpg", "iPhone 6免费送", "【望京】美梦成真+iPhone 6+美团网免费送", "0", "0", "已售452210", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/lottery/3.jpg", "SONY IL CE-5100L 标准单镜套装免费送", "【望京】美梦成真+SONY IL CE-5100L 标准单镜套装+美团网免费送", "0", "0", "已售254100", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/lottery/4.jpg", "香奈儿可可小姐香水免费送", "【望京】美梦成真+香奈儿可可小姐香水+美团网免费送", "0", "0", "已售564323", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/lottery/5.jpg", "兰蔻小黑瓶精华肌底液100ml免费送", "【望京】美梦成真+兰蔻小黑瓶精华肌底液+美团网免费送", "0", "0", "已售220614", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/lottery/6.jpg", "LG G3免费送", "【望京】美梦成真+LG G3+美团网免费送", "0", "0", "已售702500", "5km"));
        }

        /// <summary>
        /// life data
        /// </summary>
        private void loadLifeData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/life/0.jpg", "倾城恋人摄影", "【广外大街】个人写真套系：服装三套+底片全送+可拍外景", "398", "1680", "已售84", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/1.jpg", "米兰风情婚纱摄影", "【多商区】个人写真套系：服装4套+赠送精修片+可拍外景", "498", "2388", "已售250", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/2.jpg", "爱儿美儿童摄影", "【望京】儿童摄影套餐：特色外景+4套服装", "298", "2980", "已售655", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/3.jpg", "Babyface儿童摄影", "【亚运村】儿童摄影套餐+工作日拍摄加赠8寸娜米拉版画1个", "389", "3288", "已售1400", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/4.jpg", "时尚聚焦摄影", "【多商区】个人写真套餐：时尚写真服饰+专业精修照片", "198", "980", "已售223", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/5.jpg", "罗马风情婚纱影城", "【草桥】婚纱摄影套餐：新娘新郎5套服装+底片刻录光盘", "5257", "13888", "已售389", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/6.jpg", "酷迪宠物店", "【顺义】宠物美容一次+宠物洗澡+免费WiFi", "89", "150", "已售214", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/7.jpg", "大红袍茶", "【东直门】140g+清新可口+高档礼品装", "126", "240", "已售143", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/8.jpg", "红糖姜茶", "【中关村】200g+营养健康+补血+提神", "78", "150", "已售420", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/9.jpg", "罗曼蒂克", "【多商区】三生三世33朵玫瑰", "129", "238", "已售320", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/10.jpg", "郁花园鲜花速递", "【苏州桥】12朵香水百合+精品花盒", "158", "298", "已售20", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/11.jpg", "花开相爱", "【多商区】99朵玫瑰+长长久久的爱情", "380", "798", "已售34", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/12.jpg", "优衫定制西服", "【多商区】意大利品牌纯羊毛面料定制西服套装", "1999", "3689", "已售260", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/13.jpg", "西装定制", "【左家庄】衬衫+西装套餐一套", "789", "3438", "已售156", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/14.jpg", "汽车陪练", "【六里桥】手动挡六小时", "450", "530", "50", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/15.jpg", "探奇乐园", "【西红门】儿童单人通玩券+蛋糕点心免费享 ", "46", "68", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/16.jpg", "儿童主题乐园", "【多商区】儿童单人通玩票+开拓智力的软体游乐场+宝贝乐园", "140", "200", "已售548", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/17.jpg", "佳美口腔", "【多商区】洗牙套餐：免挂号费+免检查费+成人口腔全套洁牙1次", "148", "350", "已售980", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/18.jpg", "维尔口腔", "【多商区】洗牙套餐：免挂号费+免检查费+成人口腔全套洁牙1次", "108", "440", "已售2090", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/life/19.jpg", "车之友", "【十里堡】电脑洗车一次+服务到位", "20", "40", "已售1200", "5km"));
        }

        /// <summary>
        /// hotel data
        /// </summary>
        private void loadHotelData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/0.jpg", "万爱情侣酒店", "【双井】唯爱+仲夏夜之梦1晚＋免费WIFI", "329", "859", "已售42", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/1.jpg", "热带雨林风情园", "【小汤山】豪华按摩或冲浪浴缸+科勒卫浴+小型冰箱+独立无线wifi", "680", "1100", "已售58", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/2.jpg", "心动主题酒店", "【多商区】精品主题房1晚+提供有线网络和wifi", "120", "259", "已售263", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/3.jpg", "真爱主题酒店", "【公益西桥】影视主题淋浴午夜房+特色情侣/蜜月房", "708", "1200", "已售160", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/4.jpg", "概念酒店", "【五道口】主题风格+舒适大圆床住宿1晚+提供免费wifi", "680", "2030", "已售10", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/5.jpg", "远东青年旅社", "【宣武门】特惠商务房1晚+可连续入住+免费WiFi", "119", "368", "已售29", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/6.jpg", "金广快捷酒店", "【黄村】商务双床房+环境优雅+免费WIFI", "199", "279", "已售224", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/7.jpg", "国汉主题酒店", "【东直门】免费WiFi+特色情侣/蜜月房+主题任选", "280", "520", "已售143", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/8.jpg", "知春路宾馆", "【中关村】免费WiFi+100兆企业级光纤+独立院落", "198", "278", "已售18", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/9.jpg", "美途精品商务酒店", "【旧宫】高级大床房+免费宽带+舒适温馨", "268", "468", "已售78", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/10.jpg", "水晶主题酒店", "【潘家园】景观大床房/标准双人间1晚＋精选港式双人晚点/自助早餐1份", "450", "780", "已售35", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/11.jpg", "合家宾馆", "【六里桥】大床房+可连续入住+24小时热水", "168", "328", "已售120", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/12.jpg", "优优品客酒店", "【多商区】大床房1晚+免费提供WIFI", "199", "389", "已售120", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/13.jpg", "速好家酒店", "【左家庄】标准间+无线上网+空调+商务中心+停车场+行李寄存", "198", "660", "已售68", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/14.jpg", "热带雨林风情园", "【王府井】欧式私汤温泉小院周末住宿1晚+私密的温馨泡池", "780", "1300", "25", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/15.jpg", "商务旅店", "【西红门】大床房+免费WIFI+24小时热水", "138", "218", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/16.jpg", "速8酒店", "【天坛】标准间+舒适典雅+设备齐全", "88", "209", "已售23", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/17.jpg", "安平宫宾馆", "【朝阳公园】特色房+大床房+环境优越", "148", "260", "已售77", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/18.jpg", "锡华商务酒店", "【颐和园】大床房+地理位置优越+环境优美+免费WIFI", "418", "1380", "已售20", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/19.jpg", "广电酒店", "【复兴门】精品房+免费WIFI", "220", "480", "已售120", "5km"));
        }

        /// <summary>
        /// gift data
        /// </summary>
        private void loadGiftData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/0.jpg", "煲汤锅", "【望京】迷你小型+煲汤+煲粥+多功能", "127", "245", "已售800", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/1.jpg", "天乐音响", "【亚运村】电脑音箱+遥控器+音质清晰", "196", "324", "已售520", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/2.jpg", "足浴盆", "【望京】按摩+红光杀菌+水电分离+过载保护+恒温加热", "169", "289", "已售270", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/3.jpg", "超人剃须刀", "【望京-韩国新城】刮胡刀修剪器+充插电源两用+浮动式剃须刀", "90", "207", "已售320", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/4.jpg", "美的商用电饭煲", "【四道口】迷你小型+多功能电饭煲+学生宿舍可用", "358", "450", "已售27", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/5.jpg", "飞科剃须刀", "【亚运村】不锈钢+三头刀片+贴面享受", "159", "400", "已售50", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/6.jpg", "耳机", "【建国门】运动耳机+音质保真均衡+耐用", "128", "240", "已售150", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/7.jpg", "腾达路由器", "【朝阳区八里庄】300米无线增强型路由器", "76", "138", "已售130", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/8.jpg", "拉杆箱", "【五道口】万向轮+24寸行李旅行登机包+男女通用", "139", "278", "已售420", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/9.jpg", "男士钱夹", "【五道口】长款真皮钱夹+商务休闲钱夹", "119", "228", "已售780", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/10.jpg", "全自动机械手表", "【王府井】超薄+背透+镂空+防水防刮", "1798", "2398", "已售350", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/11.jpg", "博仕顿手表", "【东直门】超薄+背透+镂空+机械表", "620", "750", "已售120", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/12.jpg", "登山鞋", "【望京】低帮+户外登山鞋+男士减震", "258", "320", "已售360", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/13.jpg", "保暖睡袋", "【西直门】保暖+舒适+便携睡袋", "69", "139", "已售680", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/14.jpg", "双人床", "【唐人街】1.5米双人+木质+实用床", "780", "958", "250", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/15.jpg", "床单被罩四件套", "【大红门】特选优质面料+配以时尚花纹图案+亲肤舒适", "380", "428", "已售147", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/16.jpg", "醇然自得茶叶", "【清华园】中包+清新茶味", "79", "109", "已售1230", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/17.jpg", "士力架全家桶", "【望京新城】花生夹心巧克力+640g+实惠家庭装", "35", "69", "已售1440", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/18.jpg", "御泥坊护肤套装", "【望京】护肤保湿+洗面奶100ml+保湿乳150ml+养肤水100ml", "259", "318", "已售220", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/gift/19.jpg", "补水面膜", "【朝阳区】祛痘+去黄+美白+保湿面膜", "108", "190", "已售1210", "5km"));
        }

        /// <summary>
        /// food data
        /// </summary>
        private void loadFoodData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/food/0.jpg", "巧克力蛋糕店", "【望京】单人套餐：焦糖玛奇朵 + 蓝莓蛋糕，周一至周五免费续杯", "38", "58", "已售1120", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/1.jpg", "日本料理店", "【亚运村】双人套餐：三文鱼套餐+北极贝刺身+水果自助+CBD寿司", "188", "358", "已售650", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/2.jpg", "东北炖菜馆", "【望京】双人套餐：东北酱骨+锅包肉+东北大拉皮+猪肉炖粉条+米饭两碗", "128", "198", "已售2300", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/3.jpg", "韩国烤肉店", "【望京-韩国新城】单人套餐：韩式拌饭套餐：韩式拌饭+大麦茶+牛板筋 48元一份，韩式冷面+鸡蛋+爽口小菜 38一份", "38", "58", "已售120", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/4.jpg", "草原蒙古烤羊腿", "【四道口】双人套餐：羊腿一份（2斤半）+毛豆一盘+花生一盘+啤酒4瓶", "158", "238", "已售45", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/5.jpg", "牛排西餐厅", "【亚运村】双人套餐：牛排两份+点心三份+饮料两杯", "159.9", "400", "已售50", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/6.jpg", "风味麻辣烫", "【建国门】单人套餐：丸子+蔬菜自选", "17", "37", "已售150", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/7.jpg", "韩式烤肉店", "【朝阳区八里庄】双人套餐：石锅拌饭两份+烤肉一份+金针菇一盘+牛肉汤一份+炒年糕一份", "148", "208", "已售100", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/8.jpg", "朝鲜烤肉店", "【五道口】单人自助餐：自选", "88", "128", "已售120", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/9.jpg", "四元梅源冷饮店", "【五道口】单人套餐：芒果雪花冰一杯+香蕉酸奶冰一杯", "48", "88", "已售320", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/10.jpg", "老北京菜馆", "【王府井】双人套餐：精品烤鸭一只+鲤鱼一条+小菜一盘+两碗米饭", "238", "328", "已售350", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/11.jpg", "意大利披萨店", "【东直门】双人套餐：培根披萨一份+金枪鱼披萨一份+意面一份", "118", "288", "已售140", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/12.jpg", "老式铜锅涮肉", "【望京】双人套餐：手切鲜羊肉一盘+带鱼一份+炒菜一盘", "118", "230", "已售368", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/13.jpg", "川味烤鱼", "【西直门】双人套餐：香辣烤鱼一份（二斤）+金针菇一份+橙汁两杯+米饭两碗", "148", "268", "已售680", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/14.jpg", "海鲜自助餐厅", "【唐人街】单人套餐：三文鱼+生鱼片+扇贝+生蚝", "108", "248", "1150", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/15.jpg", "龙虾专门店", "【大红门】双人套餐：龙虾（二斤）+扇贝+海鲜+米饭两碗", "228", "408", "已售470", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/16.jpg", "大苹果烤鸭店", "【清华园】双人套餐：烤鸭一只+乌鱼蛋汤+豌豆黄+小鸭酥", "278", "418", "已售230", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/17.jpg", "韩式火锅店", "【望京新城】双人套餐：流行肉+涮羊肉+大酱汤+拌饭两份", "178", "268", "已售1240", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/18.jpg", "喜马拉雅蛋糕店", "【望京】双人套餐：蛋糕+一口酥+蛋挞+老婆饼", "108", "318", "已售2220", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/19.jpg", "东北柴锅店", "【朝阳区】双人套餐：大锅炖+小锅排骨+酸菜鱼", "238", "418", "已售410", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/20.jpg", "小小涮串", "【中关村】单人套餐：金针菇+鱼豆腐+章鱼丸+土豆片+青菜自选两种", "18", "48", "已售1500", "5km"));
        }

        /// <summary>
        /// location business data
        /// </summary>
        private void loadBusinessData()
        {
            this.list.Clear();
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/0.jpg","主题宾馆","地址：朝阳区望京街8号","328","0","","10km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/1.jpg", "汽车旅馆", "地址：朝阳区望京街花家地18号", "228", "0", "", "9km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/0.jpg", "巧克力蛋糕店", "地址：朝阳区望京街花家地北里18号", "128", "0", "", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/1.jpg", "谁家日本料理店", "地址：朝阳区望京街1号 利星行C座", "358", "0", "", "1km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/0.jpg", "好漂亮美甲", "地址：朝阳区望京街20号西门子大厦A栋", "80", "0", "", "12km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/beauty/1.jpg", "靓丽美甲店", "地址：朝阳区望京街10号A栋", "90", "0", "", "11km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/3.jpg", "像家主题酒店", "地址：朝阳区望京街100号A栋", "588", "", "", "19km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/4.jpg", "欢乐主题酒店", "地址：朝阳区望京街新世界百货4楼", "88", "", "", "3km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/2.jpg", "大东北菜馆", "地址：朝阳区望京街新世界百货4楼", "", "", "", "3km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/3.jpg", "韩国烤肉店", "地址：朝阳区望京街新世界百货4楼", "99", "", "", "3km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/0.jpg", "唱歌KTV", "地址：朝阳区望京街悠乐汇B座", "58", "", "", "6km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/1.jpg", "大家唱歌KTV", "地址：朝阳区望京街悠乐汇B座", "48", "", "", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/entertainment/2.jpg", "大脚丫按摩院", "地址：朝阳区望京街悠乐汇A座", "99", "", "", "4km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/4.jpg", "内蒙古烤羊腿", "地址：朝阳区望京街花家地北里2区", "188", "", "", "9km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/5.jpg", "牛排专门店", "地址：朝阳区望京街花家地北里1区", "128", "", "", "6km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/5.jpg", "快捷酒店", "地址：朝阳区望京街花家地北里2区", "", "", "", "10km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/6.jpg", "乐享快捷酒店", "地址：朝阳区望京街花家地北里3区", "", "", "", "9km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/7.jpg", "逍遥宫宾馆", "地址：朝阳区望京街花家地北里4区", "", "", "", "8km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/food/6.jpg", "小碗麻辣烫", "地址：朝阳区望京街悠乐汇B座", "", "", "", "5km"));
            this.list.Add(new SampleItem("ms-appx:///Assets/hotel/8.jpg", "快捷酒店", "地址：朝阳区望京街悠乐汇B座", "", "", "", "5km"));
        }

    }

    public enum Channel
    { 
        movie,
        food,
        hotel,
        entertainment,
        life,
        beauty,
        traveling,
        shop,
        gift,
        lottery,
        business,
        hall,
        cinema
    }

}
