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

        private DispatcherTimer gameTimer;

        public PlayPage()
        {
            InitializeComponent();

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Tick += GameTimer;
            gameTimer.Start();

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
                ShootBullet(playerOne, 10);


            if (e.Key == Key.J)
                moveLeftTwo = false;
            if (e.Key == Key.L)
                moveRightTwo = false;

            if (e.Key == Key.I)
                ShootBullet(playerTwo, -10);
        }
        private void ShootBullet(Rectangle player, int direction)
        {
            Rectangle bullet = new Rectangle
            {
                Width = 20,
                Height = 5,
                Fill = Brushes.Black
            };

            Canvas.SetTop(bullet, Canvas.GetTop(player) + (player.Height / 2) - (bullet.Height / 2));
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
    }
}
