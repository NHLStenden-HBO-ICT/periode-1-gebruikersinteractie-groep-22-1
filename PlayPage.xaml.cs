using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
        private Dictionary<Rectangle, int> slimeHealthDictionary = new Dictionary<Rectangle, int>();
        private Dictionary<Rectangle, int> slimeRewardDictionary = new Dictionary<Rectangle, int>();

        private List<Rectangle> bullets = new List<Rectangle>();
        private List<Rectangle> slimes = new List<Rectangle>();

        private DispatcherTimer r;
        private DispatcherTimer spawnTimer;
        private DispatcherTimer gameTimer;

        #endregion

        

        #region Constructor

        public PlayPage(string playerOneName, string playerTwoName)
        {
            InitializeComponent();

            GameCoinsAmount.Text = Values.coins.ToString();

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Tick += GameTimer;
            gameTimer.Start();

            spawnTimer = new DispatcherTimer();
            spawnTimer.Interval = TimeSpan.FromMilliseconds(500);
            spawnTimer.Tick += Waves;
            // spawnTimer.Tick += SpawnSlime;
            spawnTimer.Start();

            PlayerScreen.Focus();
        }

        #endregion

        #region Gameplay Loop

        private void GameTimer(object sender, EventArgs e)
        {
            // Player One Movement
            if (moveLeftOne && Canvas.GetLeft(playerOne) > 0 && !IsCollidingWithWall(playerOne, invisibleWall, -Values.playersMovementSpeed))
                {
                Canvas.SetLeft(playerOne, Canvas.GetLeft(playerOne) - Values.playersMovementSpeed);
                Canvas.SetLeft(playerOneSprite, Canvas.GetLeft(playerOneSprite) - Values.playersMovementSpeed); // Move image
                }

            if (moveRightOne && Canvas.GetLeft(playerOne) + playerOne.Width < PlayerScreen.ActualWidth && !IsCollidingWithWall(playerOne, invisibleWall, Values.playersMovementSpeed))
                {
                Canvas.SetLeft(playerOne, Canvas.GetLeft(playerOne) + Values.playersMovementSpeed);
                Canvas.SetLeft(playerOneSprite, Canvas.GetLeft(playerOneSprite) + Values.playersMovementSpeed); // Move image
                }

            // Player Two Movement
            if (moveLeftTwo && Canvas.GetLeft(playerTwo) > 0 && !IsCollidingWithWall(playerTwo, invisibleWall, -Values.playersMovementSpeed))
                {
                Canvas.SetLeft(playerTwo, Canvas.GetLeft(playerTwo) - Values.playersMovementSpeed);
                Canvas.SetLeft(playerTwoSprite, Canvas.GetLeft(playerTwoSprite) - Values.playersMovementSpeed); // Move image
                }

            if (moveRightTwo && Canvas.GetLeft(playerTwo) + playerTwo.Width < PlayerScreen.ActualWidth && !IsCollidingWithWall(playerTwo, invisibleWall, Values.playersMovementSpeed))
                {
                Canvas.SetLeft(playerTwo, Canvas.GetLeft(playerTwo) + Values.playersMovementSpeed);
                Canvas.SetLeft(playerTwoSprite, Canvas.GetLeft(playerTwoSprite) + Values.playersMovementSpeed); // Move image
                }

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
            // ========================================
            // || OUDE CODE, WORDT NIET MEER GEBRUIKT ||
            // ========================================

            SpawnMovingSlime(Values.slimeSpeed);
            SpawnMovingSlime(-Values.slimeSpeed);
        }

        private void SpawnMovingSlime(int slimeDirection)
        {
            // ========================================
            // || OUDE CODE, WORDT NIET MEER GEBRUIKT ||
            // ========================================

            Rectangle slime;
            int slimeHealth;
            int slimeReward;
            if (Values.slimeCounter % 3 == 0)
            {
               slime = new Rectangle
                {
                    Width = 75,
                    Height = 75,
                    Fill = Brushes.Blue
                };
                slimeHealth = Values.slime2Health;
                slimeReward = Values.slime2Reward;
            }
            else
            {
                slime = new Rectangle
                {
                    Width = 50,
                    Height = 50,
                    Fill = Brushes.Green
                };
                slimeHealth = Values.slime1Health;
                slimeReward = Values.slime1Reward;
            }

            double centerX = (PlayerScreen.ActualWidth / 2) - (slime.Width / 2);
            double centerY = (PlayerScreen.ActualHeight / 2) - (-300);

            Canvas.SetLeft(slime, centerX);
            Canvas.SetTop(slime, centerY);

            PlayerScreen.Children.Add(slime);
            slime.Tag = slimeDirection;
            slimeHealthDictionary[slime] = slimeHealth;
            slimeRewardDictionary[slime] = slimeReward;
            slimes.Add(slime);
            Values.slimeCounter++;
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
                        slimeRewardDictionary.Remove(slime);
                        slimes.RemoveAt(i);
                    }
                }
            }
        }

        #endregion

        #region Collision Detection

        private void CheckBulletSlimeCollision()
        {
            SoundPlayer soundPlayer = new SoundPlayer(@"F:\School\HBO\Periode 1\Game Interaction\8-Bit Coin Sound Effect (Copyright Free).wav");

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Rectangle bullet = bullets[i];
                for (int j = slimes.Count - 1; j >= 0; j--)
                {
                    Rectangle slime = slimes[j];

                    if (IsColliding(bullet, slime))
                    {
                        // Verwijdert bullets
                        PlayerScreen.Children.Remove(bullet);
                        bullets.RemoveAt(i);

                        // Damage aan slime
                        slimeHealthDictionary[slime] -= Values.playersDamage;

                        // Check om te kijken of slime dood is
                        if (slimeHealthDictionary[slime] <= 0)
                        {
                            soundPlayer.Play();

                            PlayerScreen.Children.Remove(slime);
                            slimeHealthDictionary.Remove(slime);
                            slimes.RemoveAt(j);
                            Values.coins += slimeRewardDictionary[slime];
                            slimeRewardDictionary.Remove(slime);
                            Values.slimesKilled++;
                            GameCoinsAmount.Text = Values.coins.ToString();

                            if (Values.slimesKilled >= 15)
                            {
                               Play.Content = new WinkelPage();
                                gameTimer.Stop();
                                spawnTimer.Stop();
                            }
                        }
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

        private bool IsCollidingWithWall(Rectangle player, Rectangle wall, double movement)
        {
            double playerLeft = Canvas.GetLeft(player) + movement;
            double playerRight = playerLeft + player.Width;

            double wallLeft = Canvas.GetLeft(wall);
            double wallRight = wallLeft + wall.Width;

            // Check if player's right side is colliding with the wall's left side
            bool isCollidingRight = playerRight > wallLeft && playerLeft < wallLeft;

            // Check if player's left side is colliding with the wall's right side
            bool isCollidingLeft = playerLeft < wallRight && playerRight > wallRight;

            // Player is colliding if either of these conditions is true
            return isCollidingLeft || isCollidingRight;
        }


        #endregion

        private void Waves(object sender, EventArgs e)
        {
            #region Slime spawn inits
            Random random = new Random();
            int spawnSlime = random.Next(100);
            int typeSlime = random.Next(100);
            int directionSlime = random.Next(2);

            bool slimeSpawning = false;
            Rectangle slime;
            int slimeWidth = 10;
            int slimeHeight = 10;
            Brush slimeFill = Brushes.Black;
            int slimeHealth = 10;
            int slimeDamage = 10;
            int slimeReward = 10;
            #endregion

            if (Values.currentWave == 0)
            {
                if (spawnSlime <= 30) // Kans dat slime spawnt
                {
                    if (typeSlime <= 70) // Kans op slime 1
                    {
                        slimeWidth = 50;
                        slimeHeight = 50;
                        slimeFill = Brushes.Green;
                        slimeHealth = Values.slime1Health;
                        slimeDamage = Values.slime1Damage;
                        slimeReward = Values.slime1Reward;
                    }
                    else
                    {
                        slimeWidth = 75;
                        slimeHeight = 75;
                        slimeFill = Brushes.Blue;
                        slimeHealth = Values.slime2Health;
                        slimeDamage = Values.slime2Damage;
                        slimeReward = Values.slime2Reward;
                    }
                    slimeSpawning = true;
                }
            }

            if (Values.currentWave == 1)
            {
                if (spawnSlime <= 50) // Kans dat slime spawnt
                {
                    if (spawnSlime <= 50) // Kans op slime 1
                    {
                        slimeWidth = 50;
                        slimeHeight = 50;
                        slimeFill = Brushes.Green;
                        slimeHealth = Values.slime1Health;
                        slimeDamage = Values.slime1Damage;
                        slimeReward = Values.slime1Reward;
                    }
                    else
                    {
                        slimeWidth = 75;
                        slimeHeight = 75;
                        slimeFill = Brushes.Blue;
                        slimeHealth = Values.slime2Health;
                        slimeDamage = Values.slime2Damage;
                        slimeReward = Values.slime2Reward;
                    }
                }
                slimeSpawning = true;
            }


            if (slimeSpawning == true)
            {
                slime = new Rectangle
                {
                    Width = slimeWidth,
                    Height = slimeHeight,
                    Fill = slimeFill
                };

                if (directionSlime == 0)
                {
                    slime.Tag = Values.slimeSpeed;
                }
                else
                {
                    slime.Tag = -Values.slimeSpeed;
                }

                double centerX = (PlayerScreen.ActualWidth / 2) - (slime.Width / 2);
                double centerY = (PlayerScreen.ActualHeight / 2) - (-300);
                Canvas.SetLeft(slime, centerX);
                Canvas.SetTop(slime, centerY);
                PlayerScreen.Children.Add(slime);
                slimes.Add(slime);

                slimeHealthDictionary[slime] = slimeHealth;
                slimeRewardDictionary[slime] = slimeReward;
                Values.slimeCounter++;
            }
        }

        private void Play_Navigated(object sender, NavigationEventArgs e)
        {
            // Handle navigation events if needed
        }
    }
}
