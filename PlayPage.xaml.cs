using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Slime_Busters
{
    public partial class PlayPage : Page
    {
        private bool moveLeftOne, moveRightOne;
        private bool moveLeftTwo, moveRightTwo;
        private double playerSpeed = 10;
        private int slimeSpeed = 5;
        private int slimeHealth = 10;
        private Dictionary<Rectangle, int> slimeHealthDictionary = new Dictionary<Rectangle, int>();


        private DispatcherTimer gameTimer;
        private DispatcherTimer spawnTimer;

        public PlayPage(string playerOneName, string playerTwoName)
        {
            InitializeComponent();

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Tick += GameTimer;
            gameTimer.Start();

            spawnTimer = new DispatcherTimer();
            spawnTimer.Interval = TimeSpan.FromSeconds(5);
            spawnTimer.Tick += SpawnSlime;
            spawnTimer.Start();

            PlayerScreen.Focus();
        }

        private void GameTimer(object sender, EventArgs e)
        {
            if (moveLeftOne && Canvas.GetLeft(playerOne) > 0)
                Canvas.SetLeft(playerOne, Canvas.GetLeft(playerOne) - playerSpeed);
            if (moveRightOne && Canvas.GetLeft(playerOne) + playerOne.Width < PlayerScreen.ActualWidth)
                Canvas.SetLeft(playerOne, Canvas.GetLeft(playerOne) + playerSpeed);

            if (moveLeftTwo && Canvas.GetLeft(playerTwo) > 0)
                Canvas.SetLeft(playerTwo, Canvas.GetLeft(playerTwo) - playerSpeed);
            if (moveRightTwo && Canvas.GetLeft(playerTwo) + playerTwo.Width < PlayerScreen.ActualWidth)
                Canvas.SetLeft(playerTwo, Canvas.GetLeft(playerTwo) + playerSpeed);

            ShootBullets();
        }

        private void KeyIsDownOne(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
                moveLeftOne = true;
            if (e.Key == Key.D)
                moveRightOne = true;

            if (e.Key == Key.J)
                moveLeftTwo = true;
            if (e.Key == Key.L)
                moveRightTwo = true;
        }

        private void KeyIsUpOne(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.A)
                moveLeftOne = false;
            if (e.Key == Key.D)
                moveRightOne = false;

            if (e.Key == Key.W)
                MakeBullets(playerOne, 10, 780);


            if (e.Key == Key.J)
                moveLeftTwo = false;
            if (e.Key == Key.L)
                moveRightTwo = false;

            if (e.Key == Key.I)
                MakeBullets(playerTwo, -10, 780);
        }
        private void MakeBullets(Rectangle player, int direction, double playerCenterHeight)
        {
            Rectangle bullet = new Rectangle
            {
                Width = 20,
                Height = 5,
                Fill = Brushes.Black
            };

            Canvas.SetTop(bullet, Canvas.GetBottom(player) + playerCenterHeight - (bullet.Height / 2));
            Canvas.SetLeft(bullet, Canvas.GetLeft(player) + (direction > 0 ? player.Width : -bullet.Width));

            PlayerScreen.Children.Add(bullet);

            bullet.Tag = direction;
        }

        private void ShootBullets()
        {
            foreach (UIElement element in PlayerScreen.Children)
            {
                if (element is Rectangle bullet && bullet.Tag != null)
                {
                    int direction = (int)bullet.Tag;

                    Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + direction);

                    if (Canvas.GetLeft(bullet) < 0 || Canvas.GetLeft(bullet) > PlayerScreen.ActualWidth)
                    {
                        PlayerScreen.Children.Remove(bullet);
                        break;
                    }
                }
            }
        }
        private void SpawnSlime(object sender, EventArgs e)
        {
            SpawnMovingSlime(slimeSpeed);

            SpawnMovingSlime(-slimeSpeed);
        }


        private void SpawnMovingSlime(int direction)
        {
            Rectangle slime = new Rectangle
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Green
            };

            double centerX = (PlayerScreen.ActualWidth / 2) - (slime.Width / 2);
            double centerY = (PlayerScreen.ActualHeight / 2) - (-300);

            Canvas.SetLeft(slime, centerX);
            Canvas.SetTop(slime, centerY);


            PlayerScreen.Children.Add(slime);

            slime.Tag = direction;

            slimeHealthDictionary[slime] = slimeHealth;
        }

        private void MoveSlime()
        {
            foreach (UIElement element in PlayerScreen.Children)
            {
                if (element is Rectangle slime && slime.Tag != null)
                {
                    int direction = (int)slime.Tag;

                    Canvas.SetLeft(slime, Canvas.GetLeft(slime) + direction);
                    if (Canvas.GetLeft(slime) < 0 || Canvas.GetLeft(slime) > PlayerScreen.ActualWidth)
                    {
                        PlayerScreen.Children.Remove(slime);
                        break;
                    }
                }
            }
        }
        private void DamageSlime(Rectangle slime, int damage)
        {
            if (slimeHealthDictionary.ContainsKey(slime))
            {
                slimeHealthDictionary[slime] -= damage;

                if (slimeHealthDictionary[slime] <= 0)
                {
                    PlayerScreen.Children.Remove(slime);
                    slimeHealthDictionary.Remove(slime);
                }
            }
        }
    }
}
