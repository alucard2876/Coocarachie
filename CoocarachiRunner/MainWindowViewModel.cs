using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace CoocarachiRunner
{
    public class MainWindowViewModel : ViewModelBase
    {
 
        private List<Coocarachi> coocarachis = new List<Coocarachi>();
        private List<Thread> runTasks = new List<Thread>();
        private Random random = new Random();
        private int columnsCount = 4;

        public MainWindowViewModel()
        {
            Canvas = new Canvas();
            Height = 800;
        }
                   
        public Canvas Canvas
        {
            get => GetProperty(() => Canvas);
            set => SetProperty(() => Canvas, value);
        }
           
        public int Height
        {
            get => GetProperty(() => Height);
            set => SetProperty(() => Height, value);
        }

        public bool CanRun() =>  !runTasks.Any() || runTasks.All(t => !t.IsAlive);

        [Command]
        public void Run()
        {
            Initialize();
            foreach (var task in runTasks)
                task.Start();
        }

        private void Initialize()
        {
            Canvas.Children.Clear();
            runTasks = new List<Thread>();

            for (int i = 0; i < 4; i++)
            {
                int currentColumn = (Height / columnsCount);  

                var coocarachie = new Coocarachi(
                    Color.FromRgb((byte)random.Next(0,255),(byte)random.Next(0,255),(byte)random.Next(0,255)),
                    new System.Windows.Point((currentColumn/2)*(i+1),(Height/2)),
                    random.Next(10,40)
                    );

                coocarachie.Ellipse.SetValue(Canvas.TopProperty, coocarachie.Corner.Y);
                coocarachie.Ellipse.SetValue(Canvas.LeftProperty, coocarachie.Corner.X);
                
                coocarachis.Add(coocarachie);

                var task = new Thread(() => RunCoocarachie(coocarachie));
                runTasks.Add(task);

                Canvas.Children.Add(coocarachie.Ellipse);
            }
            
        }

        private void RunCoocarachie(Coocarachi coocarachie)
        {
            var counter = 0;
            while ((coocarachie.Corner.Y+counter) < Height)
            {
                Thread.Sleep(coocarachie.Speed*10);

                App.Current.Dispatcher.Invoke(() =>
                {
                    
                    EventHandler handler = (sender, args) =>
                    {
                        coocarachie.Ellipse.SetValue(Canvas.TopProperty, coocarachie.Corner.Y - counter);
                        coocarachie.Ellipse.SetValue(Canvas.LeftProperty, coocarachie.Corner.X);

                        counter += coocarachie.Speed;
                    };

                    var timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += handler;
                    timer.Start();
                });
            }
            Thread.CurrentThread.Abort();
        }
    }
}
