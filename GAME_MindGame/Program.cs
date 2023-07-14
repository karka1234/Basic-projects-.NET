namespace GAME_MindGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProcessMainMenu();
        }
        static void ProcessMainMenu()
        {
            string currentUserKey = "";
            Dictionary<string, Dictionary<string, int>> palyersData = new Dictionary<string, Dictionary<string, int>>();
            Dictionary<string, Dictionary<string, int>> gameCategoryQuestionAndAnswer = new Dictionary<string, Dictionary<string, int>>();
            List<string> categoryList = new List<string>();
            gameCategoryQuestionAndAnswer = SetQAdata();
            categoryList = GetCategoryList(gameCategoryQuestionAndAnswer);
            palyersData = SetDefaultPlayerData(palyersData, categoryList);
            do
            {
                currentUserKey = Login(currentUserKey, palyersData, categoryList);
                Console.Clear();
                PrintCurrentUser(palyersData, currentUserKey);
                PrintMenu();
                switch (GetUserSelectionMainMenu())
                {
                    case "1":    ///// start game
                        Console.Clear();
                        PrintCurrentUser(palyersData, currentUserKey);
                        RunGameLogicAndPrintResult(palyersData, gameCategoryQuestionAndAnswer, categoryList, currentUserKey, 5);
                        Console.WriteLine();
                        Console.WriteLine("Betkoks mygtukas kad grizti i meniu");
                        Console.ReadKey();
                        break;
                    case "2": //Zaidimo taisykliu atvaizdavimas
                        Console.Clear();
                        PrintCurrentUser(palyersData, currentUserKey);
                        PrintGameRules();
                        Console.WriteLine();
                        Console.WriteLine("Betkoks mygtukas kad grizti i meniu");
                        Console.ReadKey();
                        break;
                    case "3": //Zaidimo rezutlatu ir dalyviu perziura
                        Console.Clear();
                        PrintCurrentUser(palyersData, currentUserKey);
                        GetUserInputAndPrintPlayersResultList(palyersData, currentUserKey);
                        Console.WriteLine();
                        Console.WriteLine("Betkoks mygtukas kad grizti i meniu");
                        Console.ReadKey();
                        break;
                    case "4":  ///atsijungiam
                        Console.Clear();
                        currentUserKey = "";
                        Console.WriteLine("Atsijungiai");
                        Console.WriteLine("Betkoks mygtukas kad testi");
                        Console.ReadKey();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Baigiam");
                        Environment.Exit(0);
                        break;
                }
            } while (true);
        }

        static string Login(string currentUserKey, Dictionary<string, Dictionary<string, int>> palyersData, List<string> categoryList)
        {
            if (currentUserKey == "")
            {
                Console.Clear();
                GetCheckSetUserLoginData(palyersData, categoryList, out currentUserKey);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Sveikas prisijunges " + currentUserKey.ToUpper() + "\t\n");
                Console.ResetColor();
                Console.WriteLine("Spauskite ENTER");
                Console.ReadKey();
            }

            return currentUserKey;
        }
        static void GetCheckSetUserLoginData(Dictionary<string, Dictionary<string, int>> palyersData, List<string> categoryList, out string currUserKey)
        {
            string firstName, lastName;
            currUserKey = "";
            do
            {
                Console.WriteLine("\t\tSveikas atvykes į 'Keptuves PROTMUSIS'\r\n");
                Console.WriteLine("Prisijungimas\r\n");
                Console.WriteLine("Ivesk savo varda");
                firstName = GetUserSelectionMainMenu();
                Console.WriteLine("Ivesk savo pavarde");
                lastName = GetUserSelectionMainMenu();
                currUserKey = firstName + " " + lastName;
                if (palyersData.ContainsKey(currUserKey))
                {
                    Console.WriteLine("Vartotojas jau registruotas: " + currUserKey);
                }
                else
                {
                    AddNewUserToPlayersDictionary(palyersData, categoryList, currUserKey);
                }
                break;
            }
            while (true);
        }

        static void AddNewUserToPlayersDictionary(Dictionary<string, Dictionary<string, int>> palyersData, List<string> categoryList, string currUserKey)
        {
            palyersData.Add(currUserKey, new Dictionary<string, int>());
            foreach (var item in categoryList)
            {
                palyersData[currUserKey].Add(item, 0);
            }
        }

        static void RunGameLogicAndPrintResult(Dictionary<string, Dictionary<string, int>> palyersData, Dictionary<string, Dictionary<string, int>> gameCategoryQuestionAndAnswer, List<string> categoryList, string currUser, int gameQuestionsCount)
        {
            int userAnsverInput = 0, helpAnswer, correctAnswers = 0, incorrectAnswers = 0;
            List<string> questionList = new List<string>();
            PrintCategoryList(categoryList);
            string userSelectedCategory = GetUserSelectedCategory(categoryList);
            Console.Clear();
            questionList = gameCategoryQuestionAndAnswer[userSelectedCategory].Keys.ToList();
            for (int gameQuestionIndex = gameQuestionsCount; gameQuestionIndex > 0; gameQuestionIndex--)
            {
                int rndIndex = GetRandomUniqQuestionID(questionList);

                PrintCurrentUser(palyersData, currUser);
                Console.WriteLine($"Klausimas {gameQuestionIndex}/{gameQuestionsCount} \r\n");
                Console.WriteLine(questionList[rndIndex]);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\r\nNorint naudotis salės pagalba įveskite 's'");
                Console.ResetColor();

                Console.WriteLine("\r\nIveskite atsakyma");
                string input = GetUserSelectionMainMenu();
                if (input == "s")
                    GetAudienceHelp(out userAnsverInput);
                else
                    userAnsverInput = Convert.ToInt32(input);

                GuessAnswerLogicAndValueChange(palyersData, gameCategoryQuestionAndAnswer, currUser, userSelectedCategory, userAnsverInput, ref correctAnswers, ref incorrectAnswers, rndIndex, questionList);
                Console.Clear();
            }
            PrintCurrentUser(palyersData, currUser);
            Console.WriteLine($"Teisingi atsakymai :{correctAnswers}, neteisingi atsakymai :{incorrectAnswers}\r\n");
            PrintUsersRezultList(palyersData);
        }

        private static void GetAudienceHelp(out int userAnsverInput)
        {
            int helpAnswer = ReturnHelpValue();
            Console.WriteLine("Salės rekomenduojamas atsakymas yra : " + helpAnswer);
            userAnsverInput = helpAnswer;
        }

        private static void GuessAnswerLogicAndValueChange(Dictionary<string, Dictionary<string, int>> palyersData, Dictionary<string, Dictionary<string, int>> gameCategoryQuestionAndAnswer, string currUser, string userSelectedCategory, int userAnsverInput, ref int correctAnswers, ref int incorrectAnswers, int rndIndex, List<string> questionList)
        {
            
            if (userAnsverInput == gameCategoryQuestionAndAnswer[userSelectedCategory][questionList[rndIndex]])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pataikei atsakymas buvo " + gameCategoryQuestionAndAnswer[userSelectedCategory][questionList[rndIndex]]);
                Console.ResetColor();
                palyersData[currUser][userSelectedCategory] += 10;
                correctAnswers++;
                Console.WriteLine("Spauskite ENTER");
                Console.ReadKey();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Nepataikei teisinga reiksme buvo : " + gameCategoryQuestionAndAnswer[userSelectedCategory][questionList[rndIndex]]);
                Console.ResetColor();
                incorrectAnswers++;
                Console.WriteLine("Spauskite ENTER");
                Console.ReadKey();
            }
        }

        private static int GetRandomUniqQuestionID(List<string> questionList)
        {
            int rndIndex = 0;
            Random random = new Random();
            List<int> rndTempArray = new List<int>();
            do
            {
                rndTempArray.Add(rndIndex);
                rndIndex = random.Next(1, questionList.Count);
            }
            while (rndTempArray.Contains(rndIndex));
            return rndIndex;
        }

        private static string GetUserSelectedCategory(List<string> categoryList)
        {
            string userSelectedCategory;
            do
            {
                int categoryId = Convert.ToInt32(GetUserSelectionMainMenu()) - 1;
                if (categoryId < categoryList.Count())
                {
                    userSelectedCategory = categoryList[categoryId];
                    break;
                }
                else
                {
                    Console.WriteLine("Tokios kategorijos nera");
                }
            }
            while (true);
            return userSelectedCategory;
        }

        static int ReturnHelpValue()
        {
            Random random = new Random();
            return random.Next(1, 4);
        }

        static void GetUserInputAndPrintPlayersResultList(Dictionary<string, Dictionary<string, int>> palyersData, string currentUserKey)
        {
            Console.WriteLine("Ivesk pasirinkima");
            Console.WriteLine("1 - Dalyviai");
            Console.WriteLine("2 - Rezultatai");
            string userSelection = GetUserSelectionMainMenu();
            if (userSelection.Equals("1"))
            {
                Console.Clear();
                PrintCurrentUser(palyersData, currentUserKey);
                PrintUsersList(palyersData);
            }
            if (userSelection.Equals("2"))
            {
                Console.Clear();
                PrintCurrentUser(palyersData, currentUserKey);
                PrintUsersRezultList(palyersData);
            }
        }
        static void PrintCategoryList(List<string> categoryList)
        {
            Console.WriteLine("Kategoriju sarasas: ");
            int index = 0;
            foreach (string category in categoryList)
            {
                index++;
                Console.WriteLine($"{index}. {category}");
            }
        }
        static void PrintGameRules()
        {
            Console.WriteLine("Sveikiname prisijungus prie 'Keptuves PROTMUSIS' programos. \r\nŠis protmūšis jums leidžia pasirinkti iš 4 klausimų kategorijų. \r\nPasirinkus kategoriją pradėsite žaidimą ir turėsite pasirinkti iš 4 galimų variantų, kuris yra jūsų klausimui teisingas atsakymas.");
        }
        static void PrintUsersRezultList(Dictionary<string, Dictionary<string, int>> playersData)
        {
            Console.WriteLine("Žaidėju rezultatai:\r\n");
            Dictionary<string, int> playerPointsSumData = new Dictionary<string, int>();
            int index = 0;
            string stars = "";
            foreach (var player in playersData)
            {
                int sum = 0;
                foreach (var playerPoints in player.Value)
                {
                    sum += Convert.ToInt32(playerPoints.Value);
                }
                playerPointsSumData.Add(player.Key, sum);
            }
            foreach (var player in playerPointsSumData.OrderByDescending(x => x.Value))
            {
                index++;
                if (index == 1)
                    stars = "*";
                else if (index == 2)
                    stars = "*";
                else if (index == 3)
                    stars = "***";
                else stars = "";
                if (index <= 3)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"{index}. {stars,4}{player.Key,-30}  \t\t{player.Value,5}");
                    Console.ResetColor();
                }
                else
                {
                    Console.ResetColor();
                    Console.WriteLine($"{index}. {stars,4}{player.Key,-30}  \t\t{player.Value,5}");
                }
            }
            Console.WriteLine();
        }
        static void PrintUsersList(Dictionary<string, Dictionary<string, int>> playersData)
        {
            Console.WriteLine("Žaidėju rezultatai:\r\n");
            foreach (var player in playersData)
            {
                int sum = 0;
                Console.WriteLine(player.Key);
                foreach (var playerPoints in player.Value)
                {
                    Console.WriteLine($"\t{playerPoints.Key} :  {playerPoints.Value}");
                    sum += playerPoints.Value;
                }
                Console.WriteLine("\t\tSuma : " + sum);
                Console.WriteLine();
            }
        }


        static List<string> GetCategoryList(Dictionary<string, Dictionary<string, int>> qaList)
        {
            List<string> gameCategoryList = new List<string>();
            foreach (var category in qaList)
            {
                gameCategoryList.Add(category.Key);
            }
            return gameCategoryList;
        }
        static int ReturnPlayerPointsCount(Dictionary<string, Dictionary<string, int>> playersData, string currUser)
        {
            int sum = 0;
            foreach (var player in playersData[currUser])
            {
                sum += Convert.ToInt32(player.Value);
            }
            return sum;
        }
        public static string GetUserSelectionMainMenu()
        {
            string? input;
            do
            {
                input = Console.ReadLine();
            }
            while (input == null || input == "");
            return input.Trim().ToLower();
        }

        static Dictionary<string, Dictionary<string, int>> SetDefaultPlayerData(Dictionary<string, Dictionary<string, int>> playersData, List<string> gameCategoryList)
        {
            playersData = new Dictionary<string, Dictionary<string, int>>()
            {
                {
                    "karolis simaitis", new Dictionary<string, int>
                    {
                        { gameCategoryList[1], 19 },
                        { gameCategoryList[3], 21 },
                    }
                },
                {
                    "karolis karolaitis", new Dictionary<string, int>
                    {
                        { gameCategoryList[2], 79 },
                        { gameCategoryList[3], 56 },
                    }
                },
                {
                    "klaidas karolaitis", new Dictionary<string, int>
                    {
                        { gameCategoryList[2], 55 },
                        { gameCategoryList[3], 2 },
                    }
                },
                {
                    "jonas jonaitis", new Dictionary<string, int>
                    {
                        { gameCategoryList[1], 88 },
                        { gameCategoryList[0], 50 },
                    }
                }
            };
            return playersData;
        }
        static void PrintCurrentUser(Dictionary<string, Dictionary<string, int>> palyersData, string currUser)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Prisijungęs : {currUser.ToUpper()}   Turi taškų : {ReturnPlayerPointsCount(palyersData, currUser)} \t\n");
            Console.ResetColor();
        }
        static void PrintMenu()
        {
            Console.WriteLine("1 - Start Game");
            Console.WriteLine("2 - Zaidimo taisykliu atvaizdavimas");
            Console.WriteLine("3 - Zaidimo rezutlatu ir dalyviu perziura");
            Console.WriteLine("4 - Atsijungimas");
            Console.WriteLine("q - Iseiti");
            Console.WriteLine("\r\nIveskite pasirinkima ir spauskite ENTER");
        }

        static Dictionary<string, Dictionary<string, int>> SetQAdata()
        {
            Dictionary<string, Dictionary<string, int>> gameCategoryQuestionAndAnswer = new Dictionary<string, Dictionary<string, int>>();

            // Kategorija 1: Geografija
            Dictionary<string, int> category1 = new Dictionary<string, int>();
            category1.Add("Kokia yra Didžioji Kinijos sostinė?\n1. Pekinas\n2. Šanchajus\n3. Honkongas\n4. Kantonas", 1); // Taisyklingas atsakymas - atsakymas 1
            category1.Add("Koks yra didžiausias Afrikos upės vandens tėkmės kiekis?\n1. Nilas\n2. Kongo\n3. Nigro\n4. Orinoko", 2); // Taisyklingas atsakymas - atsakymas 2
            category1.Add("Kas yra Italijos garsiausias pastatas?\n1. Koliziejus\n2. Leaning Tower of Pisa\n3. Pantheonas\n4. Pompejus", 3); // Taisyklingas atsakymas - atsakymas 3
            category1.Add("Kiek yra Europos Sąjungos valstybių narių?\n1. 24\n2. 27\n3. 30\n4. 32", 2); // Taisyklingas atsakymas - atsakymas 2
            category1.Add("Kas yra Didžiosios sienos kūrėjas?\n1. Aleksandras Didysis\n2. Cezaris\n3. Karalius Artūras\n4. Qin Shi Huang", 4); // Taisyklingas atsakymas - atsakymas 4
            category1.Add("Kur yra Machu Picchu?\n1. Peru\n2. Brazilija\n3. Meksika\n4. Bolivija", 1); // Taisyklingas atsakymas - atsakymas 1
            category1.Add("Kas yra Kanados nacionalinė šventė?\n1. Šv. Valentino diena\n2. Nepriklausomybės diena\n3. Kanados diena\n4. Šv. Patriko diena", 3); // Taisyklingas atsakymas - atsakymas 3
            category1.Add("Kokia yra Prancūzijos sostinė?\n1. Paryžius\n2. Berlynas\n3. Londonas\n4. Madridas", 1); // Taisyklingas atsakymas - atsakymas 1
            category1.Add("Kas yra Rusijos prezidentas?\n1. Vladimiras Putinas\n2. Dmitrijus Medvedevas\n3. Michailas Gorbačiovas\n4. Borisas Jelcinas", 1); // Taisyklingas atsakymas - atsakymas 1
            category1.Add("Kokia yra Didžioji Barjera koralų rifų ilgis?\n1. Apytiksliai 1000 km\n2. Apytiksliai 2500 km\n3. Apytiksliai 5000 km\n4. Apytiksliai 7500 km", 2); // Taisyklingas atsakymas - atsakymas 2
            category1.Add("Kuris vanduo yra pasaulio didžiausias vandens telkinys?\n1. Ramiojo vandenyno dugnas\n2. Didysis Barjera koralų rifas\n3. Kaspijos jūra\n4. Niagaros krioklys", 1); // Taisyklingas atsakymas - atsakymas 1
            category1.Add("Kas yra Brazilijos sostinė?\n1. San Paulas\n2. Rio de Žaneiras\n3. Salvadoras\n4. Brazilija neturi sostinės", 2); // Taisyklingas atsakymas - atsakymas 2

            gameCategoryQuestionAndAnswer.Add("Geografija", category1);

            // Kategorija 2: Istorija
            Dictionary<string, int> category2 = new Dictionary<string, int>();
            category2.Add("Kas buvo pirmasis Jungtinių Valstijų prezidentas?\n1. George Washington\n2. Abrahamas Linkolnas\n3. Thomas Jefferson\n4. John F. Kennedy", 1); // Taisyklingas atsakymas - atsakymas 1
            category2.Add("Kuris įvykis įvyko 1066 metais?\n1. Amerikos nepriklausomybės pasirašymas\n2. Prancūzijos revoliucija\n3. Normandijos invazija\n4. Rusijos revoliucija", 3); // Taisyklingas atsakymas - atsakymas 3
            category2.Add("Kas parašė knygą 'Karas ir taika'?\n1. Fiodoras Dostojevskis\n2. Lev Tolstoj\n3. Charles Dickens\n4. Jane Austen", 2); // Taisyklingas atsakymas - atsakymas 2
            category2.Add("Kas yra Magna Carta?\n1. Senovės Romos valdovo titulas\n2. Didžioji žaidėjo beisbolo lazda\n3. Anglijos karališkoji chartija\n4. Karaliaus krona", 3); // Taisyklingas atsakymas - atsakymas 3
            category2.Add("Kas yra Žemės laisvės statula?\n1. Didelis kalnas Škotijoje\n2. Didelis tiltas Paryžiuje\n3. Skulptūra Niujorke\n4. Katedra Prahos senamiestyje", 3); // Taisyklingas atsakymas - atsakymas 3
            category2.Add("Kas buvo Škotijos nacionalinis poetas?\n1. Robertas Burnsas\n2. Williamas Shakespeareas\n3. Charlesas Dickensas\n4. Edgaras Allan Poe", 1); // Taisyklingas atsakymas - atsakymas 1
            category2.Add("Kur įvyko Antrojo pasaulinio karo žinomieji Normandijos desantai?\n1. Prancūzija\n2. Italija\n3. Graikija\n4. Didžioji Britanija", 1); // Taisyklingas atsakymas - atsakymas 1
            category2.Add("Kas yra Pietų Afrikos Respublikos nacionalinis herojus?\n1. Nelsonas Mandela\n2. Juliusas Nyerere\n3. Robertas Mugabe\n4. Muammaras al-Gadafis", 1); // Taisyklingas atsakymas - atsakymas 1
            category2.Add("Kas buvo Rusijos caras, kai prasidėjo Pirmasis pasaulinis karas?\n1. Aleksandras II\n2. Aleksandras III\n3. Nikolajus II\n4. Vladimiras Putinas", 3); // Taisyklingas atsakymas - atsakymas 3
            category2.Add("Kas yra Amerikos nepriklausomybės deklaracijos pagrindinis autorius?\n1. George Washington\n2. Thomas Jefferson\n3. Benjaminas Franklinas\n4. John Adams", 2); // Taisyklingas atsakymas - atsakymas 2
            category2.Add("Kas buvo pirmoji pilietinės teisės gynėja JAV istorijoje?\n1. Rosa Parks\n2. Susan B. Anthony\n3. Harriet Tubman\n4. Amelia Earhart", 2); // Taisyklingas atsakymas - atsakymas 2

            gameCategoryQuestionAndAnswer.Add("Istorija", category2);

            // Kategorija 3: Mokslas
            Dictionary<string, int> category3 = new Dictionary<string, int>();
            category3.Add("Kas yra periodinė cheminių elementų lentelės pirmaeilis elementas?\n1. Vanduo\n2. Vanduo\n3. Azotas\n4. Vandenilis", 4); // Taisyklingas atsakymas - atsakymas 4
            category3.Add("Kas atrado peniciliną?\n1. Alexander Flemingas\n2. Marie Curie\n3. Isaac Newton\n4. Albert Einstein", 1); // Taisyklingas atsakymas - atsakymas 1
            category3.Add("Kas yra Didysis sprogimas (Big Bang)?\n1. Kosminis sprogimas, iš kurio atsirado Visata\n2. Didelis bombos sprogimas\n3. Šokoladinis desertas\n4. Dinamitas", 1); // Taisyklingas atsakymas - atsakymas 1
            category3.Add("Kuris organas yra atsakingas už kraujo cirkuliaciją žmogaus kūne?\n1. Širdis\n2. Smegenys\n3. Plaučiai\n4. Kepenys", 1); // Taisyklingas atsakymas - atsakymas 1
            category3.Add("Kas yra DNR?\n1. Deoksiribonukleorūgštis\n2. Didžioji naujų radinių\n3. Dviguba neigiamoji rezonancija\n4. Didžioji natūrali regeneracija", 1); // Taisyklingas atsakymas - atsakymas 1
            category3.Add("Kas yra einsteiniumas?\n1. Cheminis elementas\n2. Matematikas\n3. Palydovas\n4. Superherojus", 1); // Taisyklingas atsakymas - atsakymas 1
            category3.Add("Kas yra Teorija visko?\n1. Kosminis modelis\n2. Filosofinė koncepcija\n3. Visos teorijos apie viską\n4. Teorija apie visatą", 3); // Taisyklingas atsakymas - atsakymas 3
            category3.Add("Kas atrado elektroną?\n1. Niels Bohras\n2. Albertas Einsteinas\n3. Ernestas Rutherfordas\n4. Jamesas Clerkas Maxwellas", 3); // Taisyklingas atsakymas - atsakymas 3
            category3.Add("Kas yra pagrindinė imuninės sistemos ląstelė?\n1. T limfocitas\n2. Eritrocitas\n3. Neutrofilas\n4. Leukocitas", 1); // Taisyklingas atsakymas - atsakymas 1
            category3.Add("Kas yra pavojinga UV spinduliuotė?\n1. Saulės spinduliai\n2. Rentgeno spinduliai\n3. Mikrobangų spinduliai\n4. Kosminė spinduliuotė", 1); // Taisyklingas atsakymas - atsakymas 1
            category3.Add("Kas yra termodinamikos I-ojo dėsnio formuluotė?\n1. Energija negali būti sunaikinta ar sukuriama\n2. Naujienos sklinda greitai\n3. Skystis pasipildo užimtoje talpoje\n4. Jei į sistemą nėra atliekama darbas, tai ji įgyja energijos kiekį", 4); // Taisyklingas atsakymas - atsakymas 4
            category3.Add("Kas yra genetinė modifikacija?\n1. Organizmo keitimas genų lygmeniu\n2. Paveldėjimo dėsningumai\n3. Klonavimas\n4. Mutacijos", 1); // Taisyklingas atsakymas - atsakymas 1

            gameCategoryQuestionAndAnswer.Add("Mokslas", category3);

            // Kategorija 4: Automobiliai
            Dictionary<string, int> category4 = new Dictionary<string, int>();
            category4.Add("Kas yra pirmasis pasaulyje pagamintas automobilis?\n1. Ford Model T\n2. Mercedes-Benz Patent-Motorwagen\n3. Volkswagen Beetle\n4. Toyota Corolla", 2); // Taisyklingas atsakymas - atsakymas 2
            category4.Add("Kas yra greičiausias pasaulyje gaminamas automobilis?\n1. Bugatti Chiron Super Sport 300+\n2. Koenigsegg Jesko Absolut\n3. Hennessey Venom F5\n4. SSC Tuatara", 1); // Taisyklingas atsakymas - atsakymas 1
            category4.Add("Kuris automobilio gamintojas garsėja itin patvariais ir ilgaamžiais automobiliais?\n1. Toyota\n2. Fiat\n3. BMW\n4. Tesla", 1); // Taisyklingas atsakymas - atsakymas 1
            category4.Add("Kas yra pirmasis elektromobilis pasaulyje?\n1. Nissan Leaf\n2. Tesla Roadster\n3. Chevrolet Volt\n4. BMW i3", 2); // Taisyklingas atsakymas - atsakymas 2
            category4.Add("Kas yra garsusis sportinis italų automobilio gamintojas?\n1. Ferrari\n2. Lamborghini\n3. Maserati\n4. Alfa Romeo", 1); // Taisyklingas atsakymas - atsakymas 1
            category4.Add("Kas yra populiariausias automobilio gamintojas Kinijoje?\n1. Geely\n2. BYD\n3. Great Wall Motors\n4. Chery", 1); // Taisyklingas atsakymas - atsakymas 1
            category4.Add("Kas yra garsusis britų sportinis automobilio gamintojas?\n1. Aston Martin\n2. Jaguar\n3. Bentley\n4. Lotus", 2); // Taisyklingas atsakymas - atsakymas 2
            category4.Add("Kas yra pirmasis masiškai pagamintas automobilis?\n1. Ford Model T\n2. Volkswagen Beetle\n3. Toyota Corolla\n4. Fiat 500", 1); // Taisyklingas atsakymas - atsakymas 1
            category4.Add("Kas yra garsusis amerikiečių automobilio gamintojas?\n1. Ford\n2. Chevrolet\n3. Dodge\n4. Cadillac", 1); // Taisyklingas atsakymas - atsakymas 1
            category4.Add("Kurioje šalyje yra gaminamas automobilis BMW?\n1. Vokietijoje\n2. Italijoje\n3. Prancūzijoje\n4. JAV", 1); // Taisyklingas atsakymas - atsakymas 1
            category4.Add("Kas yra pirmasis pasaulyje pagamintas serijinis automobilis?\n1. Benz Patent-Motorwagen\n2. Ford Model T\n3. Oldsmobile Curved Dash\n4. Renault Type A", 4); // Taisyklingas atsakymas - atsakymas 4
            category4.Add("Kas yra garsusis japonų sportinis automobilio gamintojas?\n1. Toyota\n2. Nissan\n3. Honda\n4. Subaru", 3); // Taisyklingas atsakymas - atsakymas 3

            gameCategoryQuestionAndAnswer.Add("Automobiliai", category4);

            return gameCategoryQuestionAndAnswer;
        }


    }

}