using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETAbsurdV1.Code
{
    public class Homepage
    {
        public static string HomepageQuote()
        {
            Random Rnd = new Random();
            int Number = Rnd.Next(0, 14);
            string Quote = "";
            if(Number == 0)
            {
                Quote = "Beware the barrenness of a busy life."
                    + "<br/>"
                    + "Socrates";
            }
            if(Number == 1)
            {
                Quote = "“The exception is more interesting than the rule. The rule proves nothing; the exception proves everything. "
                    + "In the exception the power of real life breaks through the crust of a mechanism that has become torpid by repetition.”"
                    + "</br>"
                    + "Carl Schmitt, Political Theology: Four Chapters on the Concept of Sovereignty";
            }
            if(Number == 2)
            {
                Quote = "Tyranny naturally arises out of democracy"
                    + "<br/>"
                    + "Plato";
            }
            if(Number == 3)
            {
                Quote = "Excess of liberty, whether it lies in state or individuals, seems only to pass into excess of slavery."
                    + "<br/>"
                    + "Plato";
            }
            if(Number == 4)
            {
                Quote = "Opinion is the medium between knowledge and ignorance."
                    + "<br/>"
                    + "Plato";
            }
            if (Number == 5)
            {
                Quote = "We are what we repeatedly do. Excellence, then, is not an act, but a habit."
                    + "<br/>"
                    + "Aristotle";
            }
            if (Number == 6)
            {
                Quote = "The educated differ from the uneducated as much as the living from the dead."
                    + "<br/>"
                    + "Aristotle";
            }
            if (Number == 7)
            {
                Quote = "Except our own thoughts, there is nothing absolutely in our power."
                    + "<br/>"
                    + "René Descartes";
            }
            if (Number == 8)
            {
                Quote = "I had therefore to remove knowledge, in order to make room for belief."
                    + "<br/>"
                    + "Immanuel Kant";
            }
            if (Number == 9)
            {
                Quote = "It seems to me that a human being with the very best of intentions can do immeasurable harm, if "
                    + "he is immodest enough to wish to profit those whose spirit and will are concealed from him"
                    + "<br/>"
                    + "Nietzsche, Letter 1885";
            }
            if (Number == 10)
            {
                Quote = "“And we should consider every day lost on which we have not danced at least once. And we should call "
                    + "every truth false which was not accompanied by at least one laugh.”"
                    + "<br/>"
                    + "Nietzsche, Thus Spoke Zarathustra";
            }
            if(Number == 11)
            {
                Quote = "This paper will no doubt be found interesting by those who take an interest in it."
                    + "<br/>"
                    + "John Dalton";
            }
            if(Number == 12)
            {
                Quote = "Death does not concern us, because as long as we exist, death is not here. And when it does come, we no longer exist."
                    + "<br/>"
                    + "Epicurus";
            }
            if (Number == 13)
            {
                Quote = "Do not spoil what you have by desiring what you have not; remember that what you now have was once among the things you only hoped for."
                    + "<br/>"
                    + "Epicurus";
            }
            if (Number == 14)
            {
                Quote = "There is no such thing as justice in the abstract; it is merely a compact between men."
                    + "<br/>"
                    + "Epicurus";
            }
            if (Number == 15)
            {
                Quote = "";
            }
            if (Number == 16)
            {
                Quote = "";
            }
            if (Number == 17)
            {
                Quote = "";
            }
            if (Number == 18)
            {
                Quote = "";
            }
            if (Number == 19)
            {
                Quote = "";
            }
            if (Number == 20)
            {
                Quote = "";
            }
            return Quote;
        }
    }
}