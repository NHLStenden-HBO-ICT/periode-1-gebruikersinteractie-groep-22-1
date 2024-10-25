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
using System.Windows.Media.Animation;
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

        private Dictionary<Image, int> slimeHealthDictionary = new Dictionary<Image, int>();
        private Dictionary<Image, int> slimeDamageDictionary = new Dictionary<Image, int>();
        private Dictionary<Image, int> slimeRewardDictionary = new Dictionary<Image, int>();

        private List<Rectangle> bullets = new List<Rectangle>();
        private List<Image> slimes = new List<Image>();

        private DispatcherTimer r;
        private DispatcherTimer spawnTimer;
        private DispatcherTimer gameTimer;
        private DispatcherTimer bulletTimerOne;
        private DispatcherTimer bulletTimerTwo;

        #endregion

        

        #region Constructor

        public PlayPage(string playerOneName, string playerTwoName)
        {
            InitializeComponent();
            


            UpdatePlayerNames(playerOneName, playerTwoName);

            GameCoinsAmount.Text = Values.coins.ToString();
            WaveAmount.Text = (Values.currentWave + 1).ToString();
            bulletsOneProgressBar.Value = Values.currentBulletsOne;
            bulletsTwoProgressBar.Value = Values.currentBulletsTwo;
            bulletsOneProgressBar.Maximum = Values.maxBullets;
            bulletsTwoProgressBar.Maximum = Values.maxBullets;
            playerOneHealthBar.Value = Values.playerOneCurrentHealth;
            playerTwoHealthBar.Value = Values.playerTwoCurrentHealth;
            playerOneHealthBar.Maximum = Values.playersMaxHealth;
            playerTwoHealthBar.Maximum = Values.playersMaxHealth;

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(1);
            gameTimer.Tick += GameTimer;
            gameTimer.Start();

            spawnTimer = new DispatcherTimer();
            spawnTimer.Interval = TimeSpan.FromMilliseconds(500);
            spawnTimer.Tick += Waves;
            spawnTimer.Start();

            bulletTimerOne = new DispatcherTimer();
            bulletTimerOne.Interval = TimeSpan.FromMilliseconds(750);
            bulletTimerOne.Tick += BulletReloadOne;

            bulletTimerTwo = new DispatcherTimer();
            bulletTimerTwo.Interval = TimeSpan.FromMilliseconds(750);
            bulletTimerTwo.Tick += BulletReloadTwo;

            PlayerScreen.Focus();
        }


    private void UpdatePlayerNames(string player1Name, string player2Name)
        {
            Player1Label.Content = player1Name;
            Player2Label.Content = player2Name;
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
            SlimePlayerCollision();
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

            if (e.Key == Key.W && Values.playerOneDied == false)
                if (Values.currentBulletsOne > 0)
                {
                    MakeBullets(playerOne, 10, 700);
                    bulletTimerOne.Start();
                    Values.currentBulletsOne--;
                    bulletsOneProgressBar.Value = Values.currentBulletsOne;
                }

            if (e.Key == Key.J)
                moveLeftTwo = false;
            if (e.Key == Key.L)
                moveRightTwo = false;

            if (e.Key == Key.I && Values.playerTwoDied == false)
                if (Values.currentBulletsTwo > 0)
                {
                    MakeBullets(playerTwo, -10, 700);
                    bulletTimerTwo.Start();
                    Values.currentBulletsTwo--;
                    bulletsTwoProgressBar.Value = Values.currentBulletsTwo;
                }
        }

        #endregion

        #region Bullets

        private void MakeBullets(Rectangle player, int bulletDirection, double playerCenterHeight)
        {
            Rectangle bullet = new Rectangle
            {
                Width = 20,
                Height = 5,
                Fill = Brushes.Aqua
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

        private void BulletReloadOne(object sender, EventArgs e)
        {
            if (Values.currentBulletsOne < Values.maxBullets)
            {
                Values.currentBulletsOne++;
            }
            else
            {
                bulletTimerOne.Stop();
            }
            bulletsOneProgressBar.Value = Values.currentBulletsOne;
        }

        private void BulletReloadTwo(object sender, EventArgs e)
        {
            if (Values.currentBulletsTwo < Values.maxBullets)
            {
                Values.currentBulletsTwo++;
            }
            else
            {
                bulletTimerTwo.Stop();
            }
            bulletsTwoProgressBar.Value = Values.currentBulletsTwo;
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

            Image slime;
            int slimeHealth;
            int slimeReward;
           
            if (Values.slimeCounter % 3 == 0)
            {
                slime = new Image
                {
                    Width = 150,
                    Height = 150,
                    Source = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/groteslime.png"))                   
                };
                Canvas.SetZIndex(slime, 1);
                slimeHealth = Values.slime2Health;
                slimeReward = Values.slime2Reward;
            }
            else
            {
                slime = new Image
                {
                    Width = 100,
                    Height = 100,
                    Source = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/kleineslime.png"))
                };
                Canvas.SetZIndex(slime, 1);
                slimeHealth = Values.slime1Health;
                slimeReward = Values.slime1Reward;
            }

            double centerX = (PlayerScreen.ActualWidth / 2) - (slime.Width / 2);
            double centerY = (PlayerScreen.ActualHeight / 2) - (-300);

            Canvas.SetLeft(slime, centerX);
            Canvas.SetTop(slime, centerY);

            PlayerScreen.Children.Add(slime);
            Canvas.SetZIndex(slime, 1);
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
                Image slime = slimes[i];
                if (slime.Tag != null)
                {
                    int slimeDirection = (int)slime.Tag;
                    Canvas.SetLeft(slime, Canvas.GetLeft(slime) + slimeDirection);

                    if (Canvas.GetLeft(slime) < 0 || Canvas.GetLeft(slime) > PlayerScreen.ActualWidth)
                    {
                        PlayerScreen.Children.Remove(slime);
                        slimeHealthDictionary.Remove(slime);
                        slimeDamageDictionary.Remove(slime);
                        slimeRewardDictionary.Remove(slime);
                        slimes.RemoveAt(i);
                    }
                }
            }
        }

        private void SlimePlayerCollision()
        {
            for (int i = slimes.Count - 1; i >= 0; i--)
            {
                Image slime = slimes[i];
                if (slime.Tag != null)
                {
                    int slimeDirection = (int)slime.Tag;

                    // Check for playerOne collision and health
                    if (Canvas.GetLeft(playerOne) + playerOne.Width >= Canvas.GetLeft(slime) && Values.playerOneDied == false)
                    {
                        Values.playerOneCurrentHealth -= slimeDamageDictionary[slime];

                        // Remove the slime from the game
                        PlayerScreen.Children.Remove(slime);
                        slimeHealthDictionary.Remove(slime);
                        slimeDamageDictionary.Remove(slime);
                        slimeRewardDictionary.Remove(slime);
                        slimes.RemoveAt(i);

                        if (Values.playerOneCurrentHealth <= 0)
                        {
                            Values.playerOneDied = true;

                            // Trigger fade-out animation for playerOne
                            FadeOutAndRemovePlayer(playerOne, playerOneSprite);
                        }
                    }

                    // Check for playerTwo collision and health
                    if (Canvas.GetLeft(playerTwo) <= Canvas.GetLeft(slime) + slime.Width && Values.playerTwoDied == false)
                    {
                        Values.playerTwoCurrentHealth -= slimeDamageDictionary[slime];

                        // Remove the slime from the game
                        PlayerScreen.Children.Remove(slime);
                        slimeHealthDictionary.Remove(slime);
                        slimeDamageDictionary.Remove(slime);
                        slimeRewardDictionary.Remove(slime);
                        slimes.RemoveAt(i);

                        if (Values.playerTwoCurrentHealth <= 0)
                        {
                            Values.playerTwoDied = true;

                            // Trigger fade-out animation for playerTwo
                            FadeOutAndRemovePlayer(playerTwo, playerTwoSprite);
                        }
                    }
                }
                playerOneHealthBar.Value = Values.playerOneCurrentHealth;
                playerTwoHealthBar.Value = Values.playerTwoCurrentHealth;
            }
        }

        private void FadeOutAndRemovePlayer(UIElement player, UIElement playerSprite)
        {

            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1.0, 0.0, TimeSpan.FromSeconds(2)); // 2 seconds fade-out

            // When the animation completes, remove the player from the screen
            fadeOutAnimation.Completed += (s, e) =>
            {
                PlayerScreen.Children.Remove(player);
                PlayerScreen.Children.Remove(playerSprite);
            };

            // Start the fade-out animation
            player.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
            playerSprite.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation); // Optional: animate the sprite too
        }
        #endregion

        #region Collision Detection

        private void CheckBulletSlimeCollision()
        {
            SoundPlayer soundPlayer = new SoundPlayer("8-Bit Coin Sound Effect (Copyright Free).wav");

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Rectangle bullet = bullets[i];
                for (int j = slimes.Count - 1; j >= 0; j--)
                {
                    Image slime = slimes[j];

                    if (IsColliding(bullet, slime))
                    {
                        // Remove bullet
                        PlayerScreen.Children.Remove(bullet);
                        bullets.RemoveAt(i);

                        // Damage to slime
                        slimeHealthDictionary[slime] -= Values.playersDamage;

                        // Check if slime is dead
                        if (slimeHealthDictionary[slime] <= 0)
                        {
                            soundPlayer.Play();

                            // Trigger fade-out animation for slime
                            FadeOutAndRemoveSlime(slime);

                            // Remove slime data (health, rewards, etc.)
                            slimeHealthDictionary.Remove(slime);
                            Values.coins += slimeRewardDictionary[slime];
                            ShowCoinPopup(slimeRewardDictionary[slime], new Point(Canvas.GetLeft(slime), Canvas.GetTop(slime))); // Show popup
                            slimeRewardDictionary.Remove(slime);
                            slimes.RemoveAt(j);
                            Values.slimesKilled++;
                            GameCoinsAmount.Text = Values.coins.ToString();

                            // Check if wave is complete
                            if (Values.slimesKilled >= Values.waveRequirement)
                            {
                                Values.currentWave++;
                                Values.waveRequirement += Values.waveRequirement;
                                WaveAmount.Text = (Values.currentWave + 1).ToString();
                            }
                        }
                        break;
                    }
                }
            }
        }
        private async void ShowCoinPopup(int coins, Point position)
        {
            string coinText = coins == 1 ? "1 munt" : $"{coins} munten";

            TextBlock coinPopup = new TextBlock
            {
                Text = $"+{coinText}",
                Foreground = Brushes.Gold,
                FontSize = 24,
                Opacity = 1
            };

            // Set the position of the popup
            Canvas.SetLeft(coinPopup, position.X);
            Canvas.SetTop(coinPopup, position.Y);

            PlayerScreen.Children.Add(coinPopup);

            // Animate the popup (optional)
            double originalY = position.Y;
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(50); // Delay for a brief moment
                Canvas.SetTop(coinPopup, originalY - i * 2); // Move upwards
                coinPopup.Opacity -= 0.05; // Fade out
            }

            // Remove the popup
            PlayerScreen.Children.Remove(coinPopup);
        }


        private void FadeOutAndRemoveSlime(UIElement slime)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation(1.0, 0.0, TimeSpan.FromSeconds(1)); // 2 seconds fade-out

            // When the animation completes, remove the slime from the screen
            fadeOutAnimation.Completed += (s, e) =>
            {
                PlayerScreen.Children.Remove(slime);
            };

            // Start the fade-out animation
            slime.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation);
        }


        private bool IsColliding(Rectangle bullet, Image slime)
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


        private bool IsCollidingWithSlime(Rectangle player, Rectangle slime, double movement)
        {
            double playerLeft = Canvas.GetLeft(player) + movement;
            double playerRight = playerLeft + player.Width;

            double slimeLeft = Canvas.GetLeft(slime);
            double slimeRight = slimeLeft + slime.Width;

            bool isCollidingRight = playerRight > slimeLeft && playerLeft < slimeRight;
            bool isCollidingLeft = playerLeft < slimeRight && playerRight > slimeRight;

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
            Image slime;
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
                    if (typeSlime <= 100) // Kans op slime 1
                    {
                        slimeWidth = 100;
                        slimeHeight = 100;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/kleineslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime1Health;
                        slimeDamage = Values.slime1Damage;
                        slimeReward = Values.slime1Reward;
                    }
                    else
                    {
                        slimeWidth = 150;
                        slimeHeight = 150;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/groteslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
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
                    if (typeSlime <= 90) // Kans op slime 1
                    {
                        slimeWidth = 100;
                        slimeHeight = 100;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/kleineslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime1Health;
                        slimeDamage = Values.slime1Damage;
                        slimeReward = Values.slime1Reward;
                    }
                    else
                    {
                        slimeWidth = 150;
                        slimeHeight = 150;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/groteslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime2Health;
                        slimeDamage = Values.slime2Damage;
                        slimeReward = Values.slime2Reward;
                    }
                    slimeSpawning = true;
                }
            }

            if (Values.currentWave == 2)
            {
                if (spawnSlime <= 60) // Kans dat slime spawnt
                {
                    if (typeSlime <= 70) // Kans op slime 1
                    {
                        slimeWidth = 100;
                        slimeHeight = 100;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/kleineslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime1Health;
                        slimeDamage = Values.slime1Damage;
                        slimeReward = Values.slime1Reward;
                    }
                    else
                    {
                        slimeWidth = 150;
                        slimeHeight = 150;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/groteslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime2Health;
                        slimeDamage = Values.slime2Damage;
                        slimeReward = Values.slime2Reward;
                    }
                    slimeSpawning = true;
                }
            }

            if (Values.currentWave == 3)
            {
                if (spawnSlime <= 80) // Kans dat slime spawnt
                {
                    if (typeSlime <= 50) // Kans op slime 1
                    {
                        slimeWidth = 100;
                        slimeHeight = 100;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/kleineslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime1Health;
                        slimeDamage = Values.slime1Damage;
                        slimeReward = Values.slime1Reward;
                    }
                    else
                    {
                        slimeWidth = 150;
                        slimeHeight = 150;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/groteslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime2Health;
                        slimeDamage = Values.slime2Damage;
                        slimeReward = Values.slime2Reward;
                    }
                    slimeSpawning = true;
                }
            }

            if (Values.currentWave == 4)
            {
                if (spawnSlime <= 95) // Kans dat slime spawnt
                {
                    if (typeSlime <= 25) // Kans op slime 1
                    {
                        slimeWidth = 100;
                        slimeHeight = 100;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/kleineslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime1Health;
                        slimeDamage = Values.slime1Damage;
                        slimeReward = Values.slime1Reward;
                    }
                    else
                    {
                        slimeWidth = 150;
                        slimeHeight = 150;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/groteslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime2Health;
                        slimeDamage = Values.slime2Damage;
                        slimeReward = Values.slime2Reward;
                    }
                    slimeSpawning = true;
                }
            }

            if (Values.currentWave == 5)
            {
                if (spawnSlime <= 100) // Kans dat slime spawnt
                {
                    if (typeSlime <= 0) // Kans op slime 1
                    {
                        slimeWidth = 100;
                        slimeHeight = 100;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/kleineslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime1Health;
                        slimeDamage = Values.slime1Damage;
                        slimeReward = Values.slime1Reward;
                    }
                    else
                    {
                        slimeWidth = 150;
                        slimeHeight = 150;
                        BitmapImage slimeImage = new BitmapImage(new Uri("C:/Users/Durk/Source/Repos/periode-1-gebruikersinteractie-groep-22-1/groteslime.png"));
                        slimeFill = new ImageBrush(slimeImage);
                        slimeHealth = Values.slime2Health;
                        slimeDamage = Values.slime2Damage;
                        slimeReward = Values.slime2Reward;
                    }
                    slimeSpawning = true;
                }
            }


            if (Values.playerOneDied == false || Values.playerTwoDied == false)
            {
                if (slimeSpawning)
                {
                    slime = new Image
                    {
                        Width = slimeWidth,
                        Height = slimeHeight,
                        Source = ((ImageBrush)slimeFill).ImageSource
                    };
                    Canvas.SetZIndex(slime, 1);

                    if (directionSlime == 0)
                    {
                        slime.Tag = Values.playerOneDied ? Values.slimeSpeed : -Values.slimeSpeed;
                    }
                    else
                    {
                        slime.Tag = Values.playerTwoDied ? -Values.slimeSpeed : Values.slimeSpeed;
                    }

                    double centerX = (PlayerScreen.ActualWidth / 2) - (slime.Width / 2);
                    double centerY = (PlayerScreen.ActualHeight / 2) - (-250);
                    Canvas.SetLeft(slime, centerX);
                    Canvas.SetTop(slime, centerY);
                    PlayerScreen.Children.Add(slime);
                    Canvas.SetZIndex(slime, 1);
                    slimes.Add(slime);

                    slimeHealthDictionary[slime] = slimeHealth;
                    slimeDamageDictionary[slime] = slimeDamage;
                    slimeRewardDictionary[slime] = slimeReward;
                    Values.slimeCounter++;
                }
            }
        }

        private void Play_Navigated(object sender, NavigationEventArgs e)
        {
            // Handle navigation events if needed
        }
    }


}
