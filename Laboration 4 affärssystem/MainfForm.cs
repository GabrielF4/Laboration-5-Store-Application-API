using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Laboration_4_affärssystem
{
    public partial class MainForm : Form
    {

        BindingList<Book> BookList;
        BindingList<Movie> MovieList;
        BindingList<Game> GameList;
        BindingList<CartItem> CartList;

        List<Product> CartBuffert;

        const int BOOKS = 0;
        const int MOVIES = 1;
        const int GAMES = 2;

        const int CHECKOUTVIEW = 0;
        const int STORAGEVIEW = 1;

        int priceCartBuffer = 0;
        int productIDCounter = FileHandler.GetProductIDCounter();
        int selectedRowIndex;

        //Constructor
        public MainForm()
        {
            InitializeComponent();

            BookList = new BindingList<Book>();
            MovieList = new BindingList<Movie>();
            GameList = new BindingList<Game>();
            CartList = new BindingList<CartItem>();

            CartBuffert = new List<Product>();

            bookGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            bookGridView.MultiSelect = false;
            bookGridView.AllowUserToAddRows = false;
            bookGridView.AllowUserToDeleteRows = false;
            bookGridView.ReadOnly = true;
            bookGridView.DataSource = BookList;

            movieGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            movieGridView.MultiSelect = false;
            movieGridView.AllowUserToAddRows = false;
            movieGridView.AllowUserToDeleteRows = false;
            movieGridView.ReadOnly = true;
            movieGridView.DataSource = MovieList;

            gameGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gameGridView.MultiSelect = false;
            gameGridView.AllowUserToAddRows = false;
            gameGridView.AllowUserToDeleteRows = false;
            gameGridView.DataSource = GameList;
            gameGridView.ReadOnly = true;

            dataGridViewCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewCart.MultiSelect = false;
            dataGridViewCart.DataSource = CartList;

        }

        //Adding items to the table with the indata panel
        private void add_btn_Click(object sender, EventArgs e)
        {

            if (addItemTabControl.SelectedIndex == BOOKS)
            {
                //Abort if name or price field is empty
                if (string.IsNullOrEmpty(textBox1B.Text) || string.IsNullOrEmpty(textBox2B.Text))
                {
                    MessageBox.Show("Please enter information into the mandatory fields!");
                    return;
                }

                //Abort if incorrect input
                if (!int.TryParse(textBox2B.Text, out int price) || price < 0)
                {
                    MessageBox.Show("You have not inserted an acceptable input for the price!");
                    return;
                }

                BookList.Add(new Book()
                {
                    Name = textBox1B.Text,
                    Price = int.Parse(textBox2B.Text),
                    Author = textBox3B.Text,
                    Genre = textBox4B.Text,
                    Format = textBox5B.Text,
                    Language = textBox6B.Text,
                    ID = ++productIDCounter

                });
            }
            else if (addItemTabControl.SelectedIndex == MOVIES)
            {
                //Abort if name or price field is empty
                if (string.IsNullOrEmpty(textBox1F.Text) || string.IsNullOrEmpty(textBox2F.Text))
                {
                    MessageBox.Show("Please enter information into the mandatory fields!");
                    return;
                }

                //Abort if incorrect input
                if (!int.TryParse(textBox2F.Text, out int price) || price < 0)
                {
                    MessageBox.Show("You have not inserted an acceptable input for the price!");
                    return;
                }

                if (!int.TryParse(textBox4F.Text, out int movieLength) || movieLength <= 0)
                {
                    MessageBox.Show("Not acceptable movie length input!");
                    return;
                }

                MovieList.Add(new Movie()
                {
                    Name = textBox1F.Text,
                    Price = price,
                    Format = textBox3F.Text,
                    Playtime = movieLength + " min",
                    ID = ++productIDCounter

                });
            }
            else if (addItemTabControl.SelectedIndex == GAMES)
            {

                //Abort if name or price field is empty
                if (string.IsNullOrEmpty(textBox1G.Text) || string.IsNullOrEmpty(textBox2G.Text))
                {
                    MessageBox.Show("Please enter information into the mandatory fields!");
                    return;
                }

                //Abort if incorrect input
                if (!int.TryParse(textBox2G.Text, out int price) || price < 0)
                {
                    MessageBox.Show("You have not inserted an acceptable input for the price!");
                    return;
                }

                GameList.Add(new Game()
                {
                    Name = textBox1G.Text,
                    Price = int.Parse(textBox2G.Text),
                    Platform = textBox3G.Text,
                    ID = ++productIDCounter

                });

            }
        }

        //Removing items from teh table
        private void remove_btn_Click(object sender, EventArgs e)
        {
            if (addItemTabControl.SelectedIndex == BOOKS && bookGridView.SelectedRows.Count > 0)
            {

                DialogResult result = MessageBox.Show("Är du säker på att du vill ta bort varan?", "Säkerhetskontroll", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                BookList.RemoveAt(bookGridView.SelectedRows[0].Index);

            }
            else if (addItemTabControl.SelectedIndex == MOVIES && movieGridView.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Är du säker på att du vill ta bort varan?", "Säkerhetskontroll", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                MovieList.RemoveAt(movieGridView.SelectedRows[0].Index);

            }
            else if (addItemTabControl.SelectedIndex == GAMES && gameGridView.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Är du säker på att du vill ta bort varan?", "Säkerhetskontroll", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                {
                    return;
                }

                GameList.RemoveAt(gameGridView.SelectedRows[0].Index);

            }
        }

        //Function for switching tabs
        private void addItemTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (addItemTabControl.SelectedIndex == BOOKS)
            {
                theTabControl.SelectedIndex = BOOKS;

            }
            else if (addItemTabControl.SelectedIndex == MOVIES)
            {
                theTabControl.SelectedIndex = MOVIES;

            }
            else if (addItemTabControl.SelectedIndex == GAMES)
            {
                theTabControl.SelectedIndex = GAMES;

            }
        }

        //Cart handeling -----------------------------------------------------------------------------------------------------------------------
        private void addToCartBtn_Click(object sender, EventArgs e)
        {

            if (addItemTabControl.SelectedIndex == BOOKS && bookGridView.SelectedRows.Count > 0)
            {

                selectedRowIndex = bookGridView.SelectedRows[0].Index;

                CartList.Add(new CartItem()
                {
                    Name = BookList[selectedRowIndex].Name,
                    Price = BookList[selectedRowIndex].Price,
                    Stock = 1
                });

                CartBuffert.Add(new Product()
                {
                    Type = "Book",
                    Name = BookList[selectedRowIndex].Name,
                    Price = BookList[selectedRowIndex].Price,
                    Author = BookList[selectedRowIndex].Author,
                    Genre = BookList[selectedRowIndex].Genre,
                    Format = BookList[selectedRowIndex].Format,
                    Language = BookList[selectedRowIndex].Language,
                    ID = BookList[selectedRowIndex].ID,

                });

                priceCartBuffer += BookList[selectedRowIndex].Price;
                priceLabel.Text = priceCartBuffer.ToString();

                BookList.RemoveAt(selectedRowIndex);

            }


            else if (addItemTabControl.SelectedIndex == MOVIES && movieGridView.SelectedRows.Count > 0)
            {
                selectedRowIndex = movieGridView.SelectedRows[0].Index;

                CartList.Add(new CartItem()
                {
                    Name = MovieList[selectedRowIndex].Name,
                    Price = MovieList[selectedRowIndex].Price,
                    Stock = 1
                });

                CartBuffert.Add(new Product()
                {
                    Type = "Movie",
                    Name = MovieList[selectedRowIndex].Name,
                    Price = MovieList[selectedRowIndex].Price,
                    Format = MovieList[selectedRowIndex].Format,
                    Playtime = MovieList[selectedRowIndex].Playtime,
                    ID = MovieList[selectedRowIndex].ID,

                });

                priceCartBuffer += MovieList[selectedRowIndex].Price;
                priceLabel.Text = priceCartBuffer.ToString();

                MovieList.RemoveAt(selectedRowIndex);

            }


            else if (addItemTabControl.SelectedIndex == GAMES && gameGridView.SelectedRows.Count > 0)
            {
                selectedRowIndex = gameGridView.SelectedRows[0].Index;

                CartList.Add(new CartItem()
                {
                    Name = GameList[selectedRowIndex].Name,
                    Price = GameList[selectedRowIndex].Price,
                    Stock = 1
                });

                CartBuffert.Add(new Product()
                {
                    Type = "Game",
                    Name = GameList[selectedRowIndex].Name,
                    Price = GameList[selectedRowIndex].Price,
                    Platform = GameList[selectedRowIndex].Platform,
                    ID = GameList[selectedRowIndex].ID,

                });

                priceCartBuffer += GameList[selectedRowIndex].Price;
                priceLabel.Text = priceCartBuffer.ToString();

                GameList.RemoveAt(selectedRowIndex);

            }
        }

        private void removeFromCartBtn_Click(object sender, EventArgs e)
        {
            if (dataGridViewCart.SelectedRows.Count == 0) { return; }

            selectedRowIndex = dataGridViewCart.SelectedRows[0].Index;

            if (CartBuffert[selectedRowIndex].Type == "Book")
            {
                BookList.Add(new Book()
                {
                    Name = CartBuffert[selectedRowIndex].Name,
                    Price = CartBuffert[selectedRowIndex].Price,
                    Author = CartBuffert[selectedRowIndex].Author,
                    Genre = CartBuffert[selectedRowIndex].Genre,
                    Format = CartBuffert[selectedRowIndex].Format,
                    Language = CartBuffert[selectedRowIndex].Language,
                    ID = CartBuffert[selectedRowIndex].ID,

                });

                priceCartBuffer -= CartBuffert[selectedRowIndex].Price;
                priceLabel.Text = priceCartBuffer.ToString();

                CartBuffert.RemoveAt(selectedRowIndex);
                CartList.RemoveAt(selectedRowIndex);

            }else if(CartBuffert[selectedRowIndex].Type == "Movie")
            {
                MovieList.Add(new Movie()
                {
                    Name = CartBuffert[selectedRowIndex].Name,
                    Price = CartBuffert[selectedRowIndex].Price,
                    Format = CartBuffert[selectedRowIndex].Format,
                    Playtime = CartBuffert[selectedRowIndex].Playtime,
                    ID = CartBuffert[selectedRowIndex].ID,

                });

                priceCartBuffer -= CartBuffert[selectedRowIndex].Price;
                priceLabel.Text = priceCartBuffer.ToString();

                CartBuffert.RemoveAt(selectedRowIndex);
                CartList.RemoveAt(selectedRowIndex);


            }else if(CartBuffert[selectedRowIndex].Type == "Game")
            {
                GameList.Add(new Game()
                {
                    Name = CartBuffert[selectedRowIndex].Name,
                    Price = CartBuffert[selectedRowIndex].Price,
                    Platform = CartBuffert[selectedRowIndex].Platform,
                    ID = CartBuffert[selectedRowIndex].ID,

                });

                priceCartBuffer -= CartBuffert[selectedRowIndex].Price;
                priceLabel.Text = priceCartBuffer.ToString();

                CartBuffert.RemoveAt(selectedRowIndex);
                CartList.RemoveAt(selectedRowIndex);
            }
        }

        private void buyBtn_Click(object sender, EventArgs e)
        {
            if (CartList.Count == 0) { return; }

            MessageBox.Show($"Total Cost: {priceCartBuffer} kr\n\nThe purchase was successful!");

            CartList.Clear();
            CartBuffert.Clear();
            priceCartBuffer = 0;
            priceLabel.Text = "-";

        }

        private void resetCartBtn_Click(object sender, EventArgs e)
        {
            resetCart();
        }

        //Tabcontrol ---------------------------------------------------------------------------------------------------------------------------

        private void theTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (theTabControl.SelectedIndex == BOOKS)
            {
                addItemTabControl.SelectedIndex = BOOKS;

            }
            else if (theTabControl.SelectedIndex == MOVIES)
            {
                addItemTabControl.SelectedIndex = MOVIES;

            }
            else if (theTabControl.SelectedIndex == GAMES)
            {
                addItemTabControl.SelectedIndex = GAMES;

            }
        }

        private void tabControlBytVy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlBytVy.SelectedIndex == CHECKOUTVIEW)
            {
                bookGridView.ReadOnly = true;
                bookGridView.AllowUserToDeleteRows = false;

                movieGridView.ReadOnly = true;
                movieGridView.AllowUserToDeleteRows = false;

                gameGridView.ReadOnly = true;
                gameGridView.AllowUserToDeleteRows = false;

            }
            else if (tabControlBytVy.SelectedIndex == STORAGEVIEW)
            {
                bookGridView.ReadOnly = false;
                bookGridView.AllowUserToDeleteRows = true;

                movieGridView.ReadOnly = false;
                movieGridView.AllowUserToDeleteRows = true;

                gameGridView.ReadOnly = false;
                gameGridView.AllowUserToDeleteRows = true;

            }
        }

        //File Handling ------------------------------------------------------------------------------------------------------------------------

        private void save_btn_Click(object sender, EventArgs e)
        {
            FileHandler.SaveToFile(BookList, MovieList, GameList, productIDCounter);
            MessageBox.Show("The Tables Have Been Saved To The Database! :)");
        }

        private void load_btn_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Är du säker du vill återställa från fil utan att spara?", "?!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if(result == DialogResult.Yes)
            {
                FileHandler.LoadBooks(BookList);
                FileHandler.LoadMovies(MovieList);
                FileHandler.LoadGames(GameList);

                clearCart();
            }
        }


        //Custom Functions --------------------------------------------------------------------------------------------------------------------

        public void resetCart()
        {
            //Clear Cart
            if (CartBuffert.Count != 0)
            {
                foreach (var item in CartBuffert)
                {
                    if (item.Type == "Book")
                    {

                        BookList.Add(new Book()
                        {
                            Name = item.Name,
                            Price = item.Price,
                            Author = item.Author,
                            Genre = item.Genre,
                            Format = item.Format,
                            Language = item.Language,
                            ID = item.ID,

                        });
                    }
                    else if (item.Type == "Movie")
                    {
                        MovieList.Add(new Movie()
                        {
                            Name = item.Name,
                            Price = item.Price,
                            Format = item.Format,
                            Playtime = item.Playtime,
                            ID = item.ID,

                        });

                    }
                    else if (item.Type == "Game")
                    {
                        GameList.Add(new Game()
                        {
                            Name = item.Name,
                            Price = item.Price,
                            Platform = item.Platform,
                            ID = item.ID,

                        });
                    }
                }

                clearCart();
            }
        }

        public void clearCart()
        {
            CartBuffert.Clear();
            CartList.Clear();
            priceCartBuffer = 0;
            priceLabel.Text = "-";
        }

        public void loadOnlineDatabase()
        {
            XmlDocument db = new XmlDocument();
            WebClient client = new WebClient();

            try
            {
                client.DownloadString("https://hex.cse.kau.se/~jonavest/csharp-ap/");

            }catch(WebException e) {

                db.Load("https://hex.cse.kau.se/~jonavest/csharp-api/?action=error");
                MessageBox.Show(db.SelectSingleNode("response/error").InnerXml);
                return;
            
            }

            db.Load("https://hex.cse.kau.se/~jonavest/csharp-ap/");

            BookList.Clear();
            MovieList.Clear();
            GameList.Clear();
            clearCart();

            foreach (XmlElement xmlElement in db.DocumentElement.SelectNodes("products/book"))
            {
                BookList.Add(new Book()
                {
                    ID = int.Parse(xmlElement.SelectSingleNode("id").InnerXml),
                    Name = (xmlElement.SelectSingleNode("name") != null) ? xmlElement.SelectSingleNode("name").InnerXml : "",
                    Price = (xmlElement.SelectSingleNode("price") != null) ? int.Parse(xmlElement.SelectSingleNode("price").InnerXml) : 0,
                    Stock = (xmlElement.SelectSingleNode("stock") != null) ? int.Parse(xmlElement.SelectSingleNode("stock").InnerXml) : 0,
                    Author = (xmlElement.SelectSingleNode("author") != null) ? xmlElement.SelectSingleNode("author").InnerXml : "",
                    Genre = (xmlElement.SelectSingleNode("genre") != null) ? xmlElement.SelectSingleNode("genre").InnerXml : "",
                    Format = (xmlElement.SelectSingleNode("format") != null) ? xmlElement.SelectSingleNode("format").InnerXml : "",
                    Language = (xmlElement.SelectSingleNode("language") != null) ? xmlElement.SelectSingleNode("language").InnerXml : ""

                });
            }

            foreach (XmlElement xmlElement in db.DocumentElement.SelectNodes("products/movie"))
            {
                MovieList.Add(new Movie()
                {
                    ID = int.Parse(xmlElement.SelectSingleNode("id").InnerXml),
                    Name = (xmlElement.SelectSingleNode("name") != null) ? xmlElement.SelectSingleNode("name").InnerXml : "",
                    Price = (xmlElement.SelectSingleNode("price") != null) ? int.Parse(xmlElement.SelectSingleNode("price").InnerXml) : 0,
                    Stock = (xmlElement.SelectSingleNode("stock") != null) ? int.Parse(xmlElement.SelectSingleNode("stock").InnerXml) : 0,
                    Format = (xmlElement.SelectSingleNode("format") != null) ? xmlElement.SelectSingleNode("format").InnerXml : "",
                    Playtime = (xmlElement.SelectSingleNode("playtime") != null) ? xmlElement.SelectSingleNode("playtime").InnerXml : ""

                });
            }

            foreach (XmlElement xmlElement in db.DocumentElement.SelectNodes("products/game"))
            {
                GameList.Add(new Game()
                {
                    ID = int.Parse(xmlElement.SelectSingleNode("id").InnerXml),
                    Name = (xmlElement.SelectSingleNode("name") != null) ? xmlElement.SelectSingleNode("name").InnerXml : "",
                    Price = (xmlElement.SelectSingleNode("price") != null) ? int.Parse(xmlElement.SelectSingleNode("price").InnerXml) : 0,
                    Stock = (xmlElement.SelectSingleNode("stock") != null) ? int.Parse(xmlElement.SelectSingleNode("stock").InnerXml) : 0,
                    Platform = (xmlElement.SelectSingleNode("platform") != null) ? xmlElement.SelectSingleNode("platform").InnerXml : ""

                });
            }
        }



        //Lab 5 functions ---------------------------------------------------------------------------------------------------------------------

        //On Start
        private void EditTableForm_Load(object sender, EventArgs e)
        {
            loadOnlineDatabase();
        }

        //Exit program
        private void EditTableForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void load_from_API_btn_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Är du säker du vill hämta data från databas? (All data kommer att raderas)", "?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result != DialogResult.Yes){return;}

            loadOnlineDatabase();
        }
    }
}
