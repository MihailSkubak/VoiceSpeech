using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Speech.Recognition;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace VoiceSpeech
{
    public partial class Form1 : Form
    {
        string h;
        static string ip;
        static bool curtainsB, mistakeCurtains;
        static System.Media.SoundPlayer player;
      
        public Form1()
        {
            InitializeComponent();
        }
        static Label l;
        static bool Mistake;
        static void Connect(string Mes)
        {
            try
            {
                TcpClient tc = new TcpClient(ip, 80); 
                ASCIIEncoding asen = new ASCIIEncoding();
                NetworkStream ns = tc.GetStream();
                StreamWriter sw = new StreamWriter(ns);
                byte[] ba = asen.GetBytes(Mes);
                ns.Write(ba, 0, ba.Length);
                mistakeCurtains = true;
            }
            catch
            {
                player = new System.Media.SoundPlayer(@"..\..\ОШИБКА ПОДКЛЮЧЕНИЯ УСТРОЙСТВА (2).wav"); player.Play();
                MessageBox.Show("Ошибка подключения к серверу!");
                Mistake = true;
                mistakeCurtains = false;
            }
            if (!Mistake)
            {
                MessageBox.Show("Подключено к  " + ip + " !");
                Mistake = true;
            }
        } 
        static void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)//должна быть static
        {
            string words="";
            if (e.Result.Confidence >0.5) { l.Text = e.Result.Text;  words=e.Result.Text; }//0.7
            
            if (words == "Запрос гугл")
            {
                Process.Start("https://www.google.com/search?q="+"какая сегодня погода в Одессе");
            }
            if (words == "Открой ютуб") { Process.Start("https://www.youtube.com/");  player = new System.Media.SoundPlayer(@"..\..\ПОИСК ЗАВЕРШЕН.wav"); player.Play(); }

            if (words == "Открой гугл") { Process.Start("http://google.com");  player = new System.Media.SoundPlayer(@"..\..\ПОИСК ЗАВЕРШЕН.wav"); player.Play(); }

            if (words == "включи свет") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=OFF1/r"); }//D:\программирование\прога
            if (words == "выключи свет") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=ON1/r"); }

            if (words == "включи лампу") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=OFF2/r"); }
            if (words == "выключи лампу") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=ON2/r"); }

            if (words == "включи свет в комнате") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=OFF3/r"); }
            if (words == "выключи свет в комнате") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=ON3/r"); }

            if (words == "включи технику") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=OFF5/r"); }
            if (words == "выключи технику") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=ON5/r"); }

            if (words == "включи все") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=OFF4/r"); }
            if (words == "отключи все") {  player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); player.Play(); Connect("/LED=ON4/r"); }
           
            if (words == "открой зановески") { 
                if(curtainsB == true){
                    player = new System.Media.SoundPlayer(@"..\..\УПАКОВКА ЗАВЕРШЕНА.wav"); 
                    player.Play();
                }
                if(curtainsB == false){
                    player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); 
                    player.Play(); 
                    Connect("/LED=OFF6/r"); 
                    if(mistakeCurtains == true){
                        curtainsB=true;
                    }
                    if(mistakeCurtains == false){
                        curtainsB=false; 
                    }
                }
            }
            if (words == "закрой зановески") { 
                if(curtainsB == false){
                    player = new System.Media.SoundPlayer(@"..\..\УПАКОВКА ЗАВЕРШЕНА.wav"); 
                    player.Play();
                }
                if(curtainsB == true){
                    player = new System.Media.SoundPlayer(@"..\..\FTP-СОЕДИНЕНИЕ УСТАНОВЛЕНО.wav"); 
                    player.Play();
                    Connect("/LED=ON6/r"); 
                    if(mistakeCurtains == true){
                        curtainsB=false; 
                    }
                    if(mistakeCurtains == false){
                        curtainsB=true; 
                    }
                }
            }
        }   

        private void Form1_Shown(object sender, EventArgs e)
        {
             l = label1;
             
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-ru");// задаем язык "ru-ru"
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(ci);//Задаем обьект движка для распознавания речи, параметр обьект с языком
            sre.SetInputToDefaultAudioDevice();//место от куда распозновать речь c микрофона
           
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);//в скобки передается имя функции которое выполниется при успешном распозновании речи
           
 
            Choices numbers = new Choices();
            numbers.Add(new string[] { "открой зановески", "закрой зановески", "Запрос гугл", "Открой гугл","Открой ютуб",/*"0","1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24",*/ "включи свет", "выключи свет", "включи лампу", "выключи лампу", "включи свет в комнате", "выключи свет в комнате", "включи все", "отключи все", "включи технику", "выключи технику" });
 
            GrammarBuilder gb = new GrammarBuilder();
            gb.Culture = ci;
            gb.Append(numbers);
 
 
            Grammar g = new Grammar(gb);
            sre.LoadGrammar(g);
            
            sre.RecognizeAsync(RecognizeMode.Multiple);//Single
        }

        private void Start_Click(object sender, EventArgs e)
        {
            //Делаем таймер доступным
            MainTimer.Enabled = true;
            
            h = textBox1.Text;
            //Запускаем таймер
            MainTimer.Start();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            //Останавливаем таймер
            MainTimer.Stop();
            //Снова делаем таймер недоступным
            MainTimer.Enabled = false;

            //"Сбрасываем" текст надписи в исходное состояние
            TimeLbl.Text = "Текущее время";
            
        }

        private void MainTimer_Tick_1(object sender, EventArgs e)
        {
            //Раз в секунду будет выводиться такой текст (с текущим временем) 
            string t;
            t=TimeLbl.Text = string.Format(/*"Текущее время: {0}",*/ DateTime.Now.ToString("HH:mm:ss"));
            if (t == h)
            {
                Alarm();
                h = "Null";
            }

        }
        static void Alarm()
        {
            //Process.Start(@"D:\программирование\прога\VoiceSpeech\VoiceSpeech\02073.mp3");
            Process.Start(@"..\..\02073.mp3");
        }

        private void timerForCurtains_Tick(object sender, EventArgs e)
        {
            string open="7:00:00";
            string close= "18:00:00";
            string doNow = DateTime.Now.ToString("HH:mm:ss");
            if (doNow == open && curtainsB == false)
            {
                Connect("/LED=OFF6/r");
                if(mistakeCurtains == true){
                   curtainsB=true; 
                }
                if(mistakeCurtains == false){
                   curtainsB=false; 
                }
            }
            else if (doNow == close && curtainsB == true)
            {
                Connect("/LED=ON6/r");
                if(mistakeCurtains == true){
                    curtainsB=false; 
                }
                if(mistakeCurtains == false){
                   curtainsB=true; 
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ip = textBox2.Text;
            Mistake = false;
            Connect(" ");
        }

        private void curtains_CheckedChanged(object sender, EventArgs e)
        {
            //curtains_CheckedChanged(true, null);
            CheckBox checkBox = (CheckBox)sender; // приводим отправителя к элементу типа CheckBox
            if (checkBox.Checked == true)
            {
                MessageBox.Show(checkBox.Text + "  открыты");
                curtainsB = true;
            }
            else
            {
                MessageBox.Show(checkBox.Text + "  закрыты");
                curtainsB = false;
            }
        }
    }
}
