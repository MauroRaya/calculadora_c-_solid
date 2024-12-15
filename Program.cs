namespace Calculadora {
    internal class Program {
        static void Main(string[] args) {
            EntradaUsuario en = new EntradaUsuario();
            Calculadora calc = new Calculadora();

            calc.RegistrarOperacao('+', new Adicao());
            calc.RegistrarOperacao('-', new Subtracao());
            calc.RegistrarOperacao('*', new Multiplicacao());
            calc.RegistrarOperacao('/', new Divisao());
            calc.RegistrarOperacao('%', new Modulo());

            float numero1 = en.GetNumeroValido("Digite o 1º numero: ");
            float numero2 = en.GetNumeroValido("Digite o 2º numero: ");

            Console.WriteLine("Operações disponiveis: " + calc.GetOperacoes());
            char operacao = en.GetOperacaoValida("Digite uma operacao: ", calc);

            float resultado = calc.Calcular(operacao, numero1, numero2);
            Console.WriteLine(resultado);
        }

        class EntradaUsuario {
            public float GetNumeroValido(string prompt) {
                while (true) {
                    Console.Write(prompt);
                    string input = Console.ReadLine();

                    if (float.TryParse(input, out float num)) {
                        return num;
                    }
                }
            }

            public char GetOperacaoValida(string prompt, Calculadora calc) {
                while (true) {
                    Console.Write(prompt);
                    string input = Console.ReadLine();
                    char op;

                    if (!char.TryParse(input, out op)) {
                        continue;
                    }
                    if (!calc.GetOperacoes().Trim().Contains(op)) {
                        continue;
                    }
                    return op;
                }
            }
        }

        interface IOperacao {
            float Calcular(float num1, float num2);
        }

        class Adicao : IOperacao {
            public float Calcular(float num1, float num2) => num1 + num2;
        }

        class Subtracao : IOperacao {
            public float Calcular(float num1, float num2) => num1 - num2;
        }

        class Multiplicacao : IOperacao {
            public float Calcular(float num1, float num2) => num1 * num2;
        }

        class Divisao : IOperacao {
            public float Calcular(float num1, float num2) {
                if (num2 == 0) {
                    throw new DivideByZeroException();
                }
                return num1 / num2;
            }
        }
        class Modulo : IOperacao {
            public float Calcular(float num1, float num2) => num1 % num2;
        }

        class Calculadora {
            private readonly Dictionary<char, IOperacao> operacoes = new();

            public string GetOperacoes() => String.Join(" ", operacoes.Keys);

            public void RegistrarOperacao(char simbolo, IOperacao operacao) {
                if (operacoes.ContainsKey(simbolo)) {
                    throw new InvalidOperationException();
                }
                operacoes[simbolo] = operacao;
            }

            public float Calcular(char simbolo, float num1, float num2) {
                if (!operacoes.TryGetValue(simbolo, out var operacao)) {
                    throw new InvalidOperationException();
                }
                return operacao.Calcular(num1, num2);
            }
        }
    }
}