using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Numerics;

namespace RSA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            label3_pA.Visible = false;
            label4_qA.Visible = false;
            label5_nA.Visible = false;
            label6_FnA.Visible = false; 
            label7_eA.Visible = false;
            label8_dA.Visible = false;
            label9_open_key_A.Visible = false;
            label10_close_key_A.Visible = false;
            button3_opublikovat_keyA.Visible = false;
            label19_open_keyB_A.Visible = false;

            label18_pB.Visible = false;
            label17_qB.Visible = false;
            label16_nB.Visible = false;
            label15_FnB.Visible = false;
            label14_eB.Visible = false;
            label13_dB.Visible = false;
            label12_open_key_B.Visible = false;
            label11_close_key_B.Visible = false;
            button4_opublikovat_keyB.Visible = false;
            label20_open_keyA_B.Visible = false;

            label21.Visible = false;
            textBox1_soobshenie.Visible = false;
            button5_podpisat_soobshenie.Visible = false;
            label22_podpisannoe_soobshenie.Visible = false;
            button6_zachifrovat_soobshenie.Visible = false;
            label23_zashifrovannoe_soobshenieA.Visible = false;
            button7_peredat_podpisannoe_soobshenie.Visible = false;
            
            label24_zashifrovannoe_soobshenieB.Visible = false;
            button8_rashifrovat_soobshenie.Visible = false;
            label25_rashifrovannoe_soobshenie.Visible = false;
            button9_proverit_podpis.Visible = false;
            label26_ishodnoe_soobshenieB.Visible = false;

        }

        int pA, pB, qA, qB, nA,nB, FA, FB,eA, eB,dA, dB  ;
        BigInteger soobshenie;
        BigInteger podpisannoeSoobshenie;
        BigInteger zashifrovannoeSoobshenie;


        BigInteger rashifrovannoeSoobshenie;
        BigInteger ishodnoeSoobshenie;

        List<int> prostieChisla = get_primes(200);
        List<int> aFerma = new List<int> { };
        List<int> aRabina = new List<int> { };


        public static List<int> get_primes(int n) // решето Эратосфена (формируем простые числа)
        {

            bool[] is_prime = new bool[n + 1];
            for (int i = 2; i <= n; ++i)
                is_prime[i] = true;

            List<int> primes = new List<int>();

            for (int i = 2; i <= n; ++i)
                if (is_prime[i])
                {
                    primes.Add(i);
                    if (i * i <= n)
                        for (int j = i * i; j <= n; j += i)
                            is_prime[j] = false;
                }

            return primes;
        }

        Random rand = new Random();

        private int ModPowFerma(int x, int y, int p)
        {
            int res = 1; // Инициализация результата

            // x^y % p
            x = x % p;
            while (y > 0)
            {
                // Если y нечетное, умножаем результат на x и берем по модулю p
                if (y % 2 == 1)
                    res = (res * x) % p;

                // Уменьшаем y на половину и x возводим в квадрат, берем по модулю p
                y = y >> 1; // Эквивалентно делению y на 2
                x = (x * x) % p;
            }
            return res;
        }


        // Функция для проверки числа на простоту тестом Ферма
        private bool FermatTest(int n, int k)
        {
            aFerma.Clear();

            if (n <= 1 || n == 4) return false;
            if (n <= 3) return true;
           
            // Проверяем несколько раз
            for (int i = 0; i < k; i++)
            {
                // Выбираем случайное целое число a в диапазоне от 2 до n-1
               // Random rand = new Random();
                int a = rand.Next(2, n - 1);

                // Если a^(n-1) не равно 1 (mod n), то n - составное
                if (ModPowFerma(a, n - 1, n) != 1)
                    return false;

                aFerma.Add(a);
            }
            return true; // Если для всех случайных a выполнено условие, то n - простое с высокой вероятностью
        }

        // Функция для вычисления (x^y) % p
        private int ModPowRabin(int x, int y, int p)
        {
            int res = 1; // Инициализация результата

            // x^y % p
            x = x % p;
            while (y > 0)
            {
                // Если y нечетное, умножаем результат на x и берем по модулю p
                if (y % 2 == 1)
                    res = (res * x) % p;

                // Уменьшаем y на половину и x возводим в квадрат, берем по модулю p
                y = y >> 1; // Эквивалентно делению y на 2
                x = (x * x) % p;
            }
            return res;
        }

        // Функция для проверки числа на простоту тестом Миллера-Рабина
        private bool MillerRabinTest(int n, int k)
        {
            aRabina.Clear();
            // Проверяем несколько раз
            for (int i = 0; i < k; i++)
            {
                // Генерируем случайное основание a в диапазоне от 2 до n-2
               // Random rand = new Random();
                int a = rand.Next(2, n - 1);

                // Находим r и d, такие что n-1 = 2^r * d
                int d = n - 1;
                int r = 0;
                while (d % 2 == 0)
                {
                    d /= 2;
                    r++;
                }

                // Вычисляем a^d % n
                int x = ModPowRabin(a, d, n);

                // Если x = 1 или x = n-1, переходим к следующей итерации
                if (x == 1 || x == n - 1)
                    continue;

                // Проверяем, есть ли среди последовательных возведений x в квадрат совпадения с 1 или n-1
                for (int j = 0; j < r - 1; j++)
                {
                    x = (x * x) % n;
                    if (x == 1)
                        return false; // n - составное
                    if (x == n - 1)
                        break; // переходим к следующей итерации
                }

                // Если ни в одной из итераций не было совпадений с 1 или n-1, то n - составное
                if (x != n - 1)
                    return false;

                aRabina.Add(a);
            }
            return true; // Если для всех случайных оснований a выполнились условия, то n - простое с высокой вероятностью
        }



        // Расширенный алгоритм Евклида для нахождения обратного элемента числа по модулю
        private int ExtendedEuclidean(int a, int m, out int x, out int y)
        {
            if (m == 0)
            {
                x = 1;
                y = 0;
                return a;
            }

            int x1, y1;
            int gcd = ExtendedEuclidean(m, a % m, out x1, out y1);

            x = y1;
            y = x1 - (a / m) * y1;

            return gcd;
        }


        // Функция для нахождения обратного элемента по модулю
        private int ModInverse(int a, int m)
        {
            int x, y;
            int gcd = ExtendedEuclidean(a, m, out x, out y);

            if (gcd != 1)
            {
                return 0;
                //throw new ArgumentException("Число не имеет обратного элемента по модулю.");
            }

            return (x % m + m) % m;
        }

        
       
        
        private void button1_keysA_Click(object sender, EventArgs e)
        {
            label3_pA.Visible = true;
            label4_qA.Visible = true;
            label5_nA.Visible = true;
            label6_FnA.Visible = true;
            label7_eA.Visible = true;
            label8_dA.Visible = true;
            label9_open_key_A.Visible = true;
            label10_close_key_A.Visible = true;
            button3_opublikovat_keyA.Visible = true;

            int temp;
            temp =  rand.Next(prostieChisla.Count);
            pA = prostieChisla[temp];
            label3_pA.Text = $"p={pA}";

            if (FermatTest(pA, 3) && MillerRabinTest(pA, 8))
            {
                try
                {
                    if (aRabina.Count == 3)
                    {
                        MessageBox.Show($"Число p = {pA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} \nТест Миллера-Рабина с а = {aRabina[2]}");
                    }
                    if (aRabina.Count == 2)
                    {
                        MessageBox.Show($"Число p = {pA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} ");
                    }
                    if (aRabina.Count == 1)
                    {
                        MessageBox.Show($"Число p = {pA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} ");
                    }
                    if (aRabina.Count == 0)
                    {
                        MessageBox.Show($"Число p = {pA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " );
                    }

                    //MessageBox.Show($"Число p = {pA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                    //    $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} \nТест Миллера-Рабина с а = {aRabina[2]}" );

                }
                catch
                {
                    MessageBox.Show($"Число p = {pA} простое");
                }

            }

            temp = rand.Next(prostieChisla.Count);
            qA = prostieChisla[temp];
            while (pA == qA)
            {
                temp = rand.Next(prostieChisla.Count);
                qA = prostieChisla[temp];
            }
            label4_qA.Text = $"q={qA}";

            if (FermatTest(qA, 3) && MillerRabinTest(qA, 8))
            {
                try
                {
                    if (aRabina.Count >= 3)
                    {
                        MessageBox.Show($"Число q = {qA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} \nТест Миллера-Рабина с а = {aRabina[2]}");
                    }
                    if (aRabina.Count == 2)
                    {
                        MessageBox.Show($"Число q = {qA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} ");
                    }
                    if (aRabina.Count == 1)
                    {
                        MessageBox.Show($"Число q = {qA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} ");
                    }
                    if (aRabina.Count == 0)
                    {
                        MessageBox.Show($"Число q = {qA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} ");
                    }

                    //MessageBox.Show($"Число p = {pA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                    //    $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} \nТест Миллера-Рабина с а = {aRabina[2]}" );

                }
                catch
                {
                    MessageBox.Show($"Число q = {qA} простое");
                }

            }

            nA = pA * qA;
            label5_nA.Text = $"n={nA}";

            FA = (pA - 1) * (qA - 1);
            label6_FnA.Text = $"F(n)={FA}";

            List<int> forE = get_primes(FA/2);

            temp = rand.Next(forE.Count);
            
            eA = forE[temp];

            dA = ModInverse(eA, FA);
            while (dA == 0)
            {
                temp = rand.Next(forE.Count);
                eA = forE[temp];
                dA = ModInverse(eA, dA);
            }
            label7_eA.Text = $"e={eA}";

            label8_dA.Text = $"d={dA}";

            label9_open_key_A.Text = $"Открытый ключ: ({eA}, {nA})";
            label10_close_key_A.Text = $"Закрытый ключ: ({dA}, {nA})";


        }

        private void button2_keysB_Click(object sender, EventArgs e)
        {
            label18_pB.Visible = true;
            label17_qB.Visible = true;
            label16_nB.Visible = true;
            label15_FnB.Visible = true;
            label14_eB.Visible = true;
            label13_dB.Visible = true;
            label12_open_key_B.Visible = true;
            label11_close_key_B.Visible = true;
            button4_opublikovat_keyB.Visible = true;


            int temp;
            temp = rand.Next(prostieChisla.Count);
            pB = prostieChisla[temp];
            label18_pB.Text = $"p={pB}";

            if (FermatTest(pB, 3) && MillerRabinTest(pB, 5))
            {
                try
                {
                    if (aRabina.Count == 3)
                    {
                        MessageBox.Show($"Число p = {pB} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} \nТест Миллера-Рабина с а = {aRabina[2]}");
                    }
                    if (aRabina.Count == 2)
                    {
                        MessageBox.Show($"Число p = {pB} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} ");
                    }
                    if (aRabina.Count == 1)
                    {
                        MessageBox.Show($"Число p = {pB} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} ");
                    }
                    if (aRabina.Count == 0)
                    {
                        MessageBox.Show($"Число p = {pB} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} ");
                    }

                    //MessageBox.Show($"Число p = {pA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                    //    $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} \nТест Миллера-Рабина с а = {aRabina[2]}" );

                }
                catch
                {
                    MessageBox.Show($"Число p = {pB} простое");
                }

            }

            temp = rand.Next(prostieChisla.Count);
            qB = prostieChisla[temp];
            while (pB == qB)
            {
                temp = rand.Next(prostieChisla.Count);
                qB = prostieChisla[temp];
            }
            label17_qB.Text = $"q={qB}";

            if (FermatTest(qB, 3) && MillerRabinTest(qB, 5))
            {
                try
                {
                    if (aRabina.Count >= 3)
                    {
                        MessageBox.Show($"Число q = {qB} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} \nТест Миллера-Рабина с а = {aRabina[2]}");
                    }
                    if (aRabina.Count == 2)
                    {
                        MessageBox.Show($"Число q = {qB} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} ");
                    }
                    if (aRabina.Count == 1)
                    {
                        MessageBox.Show($"Число q = {qB} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                       $"\nТест Миллера-Рабина с а = {aRabina[0]} ");
                    }
                    if (aRabina.Count == 0)
                    {
                        MessageBox.Show($"Число q = {qB} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} ");
                    }

                    //MessageBox.Show($"Число p = {pA} простое \nТест Ферма с a = {aFerma[0]} \nТест Ферма с a = {aFerma[1]} \nТест Ферма с a = {aFerma[2]} " +
                    //    $"\nТест Миллера-Рабина с а = {aRabina[0]} \nТест Миллера-Рабина с а = {aRabina[1]} \nТест Миллера-Рабина с а = {aRabina[2]}" );

                }
                catch
                {
                    MessageBox.Show($"Число q = {qB} простое");
                }

            }

            nB = pB * qB;
            label16_nB.Text = $"n={nB}";

            FB = (pB - 1) * (qB - 1);
            label15_FnB.Text = $"F(n)={FB}";

            List<int> forE = get_primes(FB / 2);

            temp = rand.Next(forE.Count);

            eB = forE[temp];

            dB = ModInverse(eB, FB);
            while (dB == 0)
            {
                temp = rand.Next(forE.Count);
                eB = forE[temp];
                dB = ModInverse(eB, dB);
            }
            label14_eB.Text = $"e={eB}";

            label13_dB.Text = $"d={dB}";

            label12_open_key_B.Text = $"Открытый ключ: ({eB}, {nB})";
            label11_close_key_B.Text = $"Закрытый ключ: ({dB}, {nB})";
        }

        private void button3_opublikovat_keyA_Click(object sender, EventArgs e)
        {
            label20_open_keyA_B.Visible = true;
            label20_open_keyA_B.Text = $"Открытый ключ пользователя А: ({eA}, {nA})";
        }

        private void button4_opublikovat_keyB_Click(object sender, EventArgs e)
        {
            label19_open_keyB_A.Visible = true;
            label19_open_keyB_A.Text = $"Открытый ключ пользователя B: ({eB}, {nB})";
            label21.Visible = true;
            textBox1_soobshenie.Visible = true;
            button5_podpisat_soobshenie.Visible = true;
        }
        private void button5_podpisat_soobshenie_Click(object sender, EventArgs e)
        {
            if (textBox1_soobshenie.Text != "")
            {
                soobshenie = Convert.ToInt32(textBox1_soobshenie.Text);
                podpisannoeSoobshenie =  BigInteger.Pow(soobshenie, dA) % nA;
                label22_podpisannoe_soobshenie.Text = $"Подписанное сообщение: {podpisannoeSoobshenie}";

                label22_podpisannoe_soobshenie.Visible = true;
                button6_zachifrovat_soobshenie.Visible = true;
            }
        }

        private void button6_zachifrovat_soobshenie_Click(object sender, EventArgs e)
        {
            
            zashifrovannoeSoobshenie = BigInteger.Pow(podpisannoeSoobshenie, eB) % nB;
            label23_zashifrovannoe_soobshenieA.Text = $"Зашифрованное сообщение: {zashifrovannoeSoobshenie}";

            label23_zashifrovannoe_soobshenieA.Visible = true;
            button7_peredat_podpisannoe_soobshenie.Visible = true;
        }

        private void button7_peredat_podpisannoe_soobshenie_Click(object sender, EventArgs e)
        {
            label24_zashifrovannoe_soobshenieB.Text = $"Зашифрованное подписанное сообщение: {zashifrovannoeSoobshenie}";
            label24_zashifrovannoe_soobshenieB.Visible = true;
            button8_rashifrovat_soobshenie.Visible = true;
            
        }
        private void button8_rashifrovat_soobshenie_Click(object sender, EventArgs e)
        {
            label25_rashifrovannoe_soobshenie.Visible = true;
            button9_proverit_podpis.Visible = true;
            
            rashifrovannoeSoobshenie = BigInteger.Pow(zashifrovannoeSoobshenie, dB) % nB;
            label25_rashifrovannoe_soobshenie.Text = $"Расшифрованное сообщение: {rashifrovannoeSoobshenie}";
        }

        private void button9_proverit_podpis_Click(object sender, EventArgs e)
        {
            ishodnoeSoobshenie = BigInteger.Pow(rashifrovannoeSoobshenie, eA) % nA;
            label26_ishodnoe_soobshenieB.Text = $"Исходное сообщение: {ishodnoeSoobshenie}";
            label26_ishodnoe_soobshenieB.Visible = true;
            if (Convert.ToString(ishodnoeSoobshenie) == textBox1_soobshenie.Text)
            {
                MessageBox.Show("Подпись верна!");
            }
            else
            {
                MessageBox.Show("Подпись не верна!");
            }
        }
    }
}
