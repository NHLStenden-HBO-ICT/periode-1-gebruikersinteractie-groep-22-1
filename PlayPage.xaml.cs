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
        #region Variables

        private bool moveLeftOne, moveRightOne;
        private bool moveLeftTwo, moveRightTwo;
        private double playerSpeed = 10;
        private int slimeSpeed = 5;
        private int slimeHealth = 10;
        private Dictionary<Rectangle, int> slimeHealthDictionary = new Dictionary<Rectangle, int>();

        private List<Rectangle> bullets = new List<Rectangle>();
        private List<Rectangle> slimes = new List<Rectangle>();

        private DispatcherTimer gameTimer;
        private DispatcherTimer spawnTimer;

        #endregion

        #region Constructor

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

        #endregion

        #region Gameplay Loop

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
            MoveSlime();
            CheckBulletSlimeCollision();
        }

        #endregion

        #region Player movement

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

        #endregion

        #region Bullets

        private void MakeBullets(Rectangle player, int bulletDirection, double playerCenterHeight)
        {
            Rectangle bullet = new Rectangle
            {
                Width = 20,
                Height = 5,
                Fill = Brushes.Black
            };

            Canvas.SetTop(bullet, Canvas.GetBottom(player) + playerCenterHeight - (bullet.Height / 2));
            Canvas.SetLeft(bullet, Canvas.GetLeft(player) + (bulletDirection > 0 ? player.Width : -bullet.Width));

            PlayerScreen.Children.Add(bullet);
            bullet.Tag = bulletDirection;
            bullets.Add(bullet);
        }

        private void ShootBullets()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Rectangle bullet = bullets[i];
                if (bullet.Tag != null)
                {
                    int bulletDirection = (int)bullet.Tag;
                    Canvas.SetLeft(bullet, Canvas.GetLeft(bullet) + bulletDirection);

                    if (Canvas.GetLeft(bullet) < 0 || Canvas.GetLeft(bullet) > PlayerScreen.ActualWidth)
                    {
                        PlayerScreen.Children.Remove(bullet);
                        bullets.RemoveAt(i);
                    }
                }
            }
        }

        #endregion

        #region Slimes

        private void SpawnSlime(object sender, EventArgs e)
        {
            SpawnMovingSlime(slimeSpeed);
            SpawnMovingSlime(-slimeSpeed);
        }

        private void SpawnMovingSlime(int slimeDirection)
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
            slime.Tag = slimeDirection;
            slimeHealthDictionary[slime] = slimeHealth;
            slimes.Add(slime);
        }

        private void MoveSlime()
        {
            for (int i = slimes.Count - 1; i >= 0; i--)
            {
                Rectangle slime = slimes[i];
                if (slime.Tag != null)
                {
                    int slimeDirection = (int)slime.Tag;
                    Canvas.SetLeft(slime, Canvas.GetLeft(slime) + slimeDirection);

                    if (Canvas.GetLeft(slime) < 0 || Canvas.GetLeft(slime) > PlayerScreen.ActualWidth)
                    {
                        PlayerScreen.Children.Remove(slime);
                        slimeHealthDictionary.Remove(slime);
                        slimes.RemoveAt(i);
                    }
                }
            }
        }

        #endregion

        #region Collision Detection

        private void CheckBulletSlimeCollision()
        {
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Rectangle bullet = bullets[i];
                for (int j = slimes.Count - 1; j >= 0; j--)
                {
                    Rectangle slime = slimes[j];

                    if (IsColliding(bullet, slime))
                    {
                        PlayerScreen.Children.Remove(bullet);
                        PlayerScreen.Children.Remove(slime);

                        bullets.RemoveAt(i);
                        slimeHealthDictionary.Remove(slime);
                        slimes.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        private bool IsColliding(Rectangle bullet, Rectangle slime)
        {
            Rect bulletRect = new Rect(Canvas.GetLeft(bullet), Canvas.GetTop(bullet), bullet.Width, bullet.Height);
            Rect slimeRect = new Rect(Canvas.GetLeft(slime), Canvas.GetTop(slime), slime.Width, slime.Height);

            return bulletRect.IntersectsWith(slimeRect);
        }

        #endregion

        #region Invisible Wall
        private void CheckPlayerWallCollision()
        {

        }
        #endregion
    }
}

