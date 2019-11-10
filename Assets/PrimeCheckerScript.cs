using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class PrimeCheckerScript : MonoBehaviour
{

    //Logging, not original
    static int moduleCounter = 1;
    int moduleID;
    private bool moduleSolved;

    //animating var, not original
    private bool animating = false;

    private void Start()
	{
        moduleID = moduleCounter++;
        this.maintext.text = string.Empty;
		this.init();
	}

	private void Update()
	{
	}

	private void init()
	{
		KMBombModule component = base.GetComponent<KMBombModule>();
		component.OnActivate = (KMBombModule.KMModuleActivateEvent)Delegate.Combine(component.OnActivate, new KMBombModule.KMModuleActivateEvent(this.OnActivate));
		KMSelectable component2 = base.GetComponent<KMSelectable>();
		component2.OnCancel = (KMSelectable.OnCancelHandler)Delegate.Combine(component2.OnCancel, new KMSelectable.OnCancelHandler(this.OnCancel));
		KMSelectable component3 = base.GetComponent<KMSelectable>();
		component3.OnLeft = (Action)Delegate.Combine(component3.OnLeft, new Action(this.OnLeft));
		KMSelectable component4 = base.GetComponent<KMSelectable>();
		component4.OnRight = (Action)Delegate.Combine(component4.OnRight, new Action(this.OnRight));
		KMSelectable component5 = base.GetComponent<KMSelectable>();
		component5.OnSelect = (Action)Delegate.Combine(component5.OnSelect, new Action(this.OnSelect));
		KMSelectable component6 = base.GetComponent<KMSelectable>();
		component6.OnDeselect = (Action)Delegate.Combine(component6.OnDeselect, new Action(this.OnDeselect));
		KMSelectable component7 = base.GetComponent<KMSelectable>();
		component7.OnHighlight = (Action)Delegate.Combine(component7.OnHighlight, new Action(this.OnHighlight));
		KMSelectable kmselectable = this.buttons[0];
		kmselectable.OnInteract = (KMSelectable.OnInteractHandler)Delegate.Combine(kmselectable.OnInteract, new KMSelectable.OnInteractHandler(delegate()
		{
			this.pressbuttonprime();
			return false;
		}));
		KMSelectable kmselectable2 = this.buttons[1];
		kmselectable2.OnInteract = (KMSelectable.OnInteractHandler)Delegate.Combine(kmselectable2.OnInteract, new KMSelectable.OnInteractHandler(delegate()
		{
			this.pressbuttonnotprime();
			return false;
		}));
		KMSelectable kmselectable3 = this.buttons[0];
		kmselectable3.OnInteractEnded = (Action)Delegate.Combine(kmselectable3.OnInteractEnded, new Action(this.OnRelease));
		KMSelectable kmselectable4 = this.buttons[1];
		kmselectable4.OnInteractEnded = (Action)Delegate.Combine(kmselectable4.OnInteractEnded, new Action(this.OnRelease));
	}

	public void newprime()
	{
		this.currentindex++;
		if (this.currentindex == 4)
		{
            //Debug.Log("end");
            moduleSolved = true;
            Debug.LogFormat("[Prime Checker #{0}] Module Disarmed.", moduleID);
            this.maintext.text = string.Empty;
            base.GetComponent<KMBombModule>().HandlePass();
		}
		else
		{
			this.maintext.GetComponent<TextMesh>().text = this.generatenumber().ToString();
            Debug.LogFormat("[Prime Checker #{0}] <Stage {1}> Generated Number: {2}", moduleID, this.currentindex, this.maintext.GetComponent<TextMesh>().text);
            Debug.LogFormat("[Prime Checker #{0}] <Stage {1}> Number is prime: {2}", moduleID, this.currentindex, this.iscurrentprime);
        }
	}

	public int generatenumber()
	{
		int num = UnityEngine.Random.Range(1, 100);
		if (num < 50)
		{
			//Debug.Log("yesprime");
			this.iscurrentprime = true;
			return int.Parse(this.primenumbers.Split(new char[]
			{
				'\n'
			})[UnityEngine.Random.Range(1, this.primenumbers.Split(new char[]
			{
				'\n'
			}).Length)]);
		}
		//Debug.Log("noprime");
		this.iscurrentprime = false;
		return int.Parse(this.nonprimenumbers.Split(new char[]
		{
			'\n'
		})[UnityEngine.Random.Range(1, this.nonprimenumbers.Split(new char[]
		{
			'\n'
		}).Length)]);
	}

	private void pressbuttonprime()
	{
        if(moduleSolved != true && animating != true)
        {
            StartCoroutine(animateButton(this.buttons[0]));
            buttons[0].AddInteractionPunch(0.5f);
            this.audioRef = base.GetComponent<KMAudio>().PlayGameSoundAtTransformWithRef(KMSoundOverride.SoundEffect.ButtonPress, base.transform);
            if (this.iscurrentprime)
            {
                //Debug.Log("test3");
                Debug.LogFormat("[Prime Checker #{0}] <Stage {1}> Prime pressed, that is correct.", moduleID, this.currentindex);
                this.newprime();
                this.LEDOFF[this.currentindex - 2].SetActive(false);
                this.LEDCORRECT[this.currentindex - 2].SetActive(true);
            }
            else
            {
                //Debug.Log("test4");
                Debug.LogFormat("[Prime Checker #{0}] <Stage {1}> Prime pressed, that is incorrect. Strike.", moduleID, this.currentindex);
                base.GetComponent<KMBombModule>().HandleStrike();
            }
        }
	}

	private void OnActivate()
	{
		this.newprime();
	}

	private bool OnCancel()
	{
		//Debug.Log("ExampleModule2 cancel.");
		return true;
	}

	private void OnDeselect()
	{
		//Debug.Log("ExampleModule2 OnDeselect.");
	}

	private void OnLeft()
	{
		//Debug.Log("ExampleModule2 OnLeft.");
	}

	private void OnRight()
	{
		//Debug.Log("ExampleModule2 OnRight.");
	}

	private void OnSelect()
	{
		//Debug.Log("ExampleModule2 OnSelect.");
	}

	private void OnHighlight()
	{
		//Debug.Log("ExampleModule2 OnHighlight.");
	}

	private void pressbuttonnotprime()
	{
        if(moduleSolved != true && animating != true)
        {
            StartCoroutine(animateButton(this.buttons[1]));
            buttons[1].AddInteractionPunch(0.5f);
            this.audioRef = base.GetComponent<KMAudio>().PlayGameSoundAtTransformWithRef(KMSoundOverride.SoundEffect.ButtonPress, base.transform);
            if (!this.iscurrentprime)
            {
                //Debug.Log("test");
                Debug.LogFormat("[Prime Checker #{0}] <Stage {1}> Not Prime pressed, that is correct.", moduleID, this.currentindex);
                this.newprime();
                this.LEDOFF[this.currentindex - 2].SetActive(false);
                this.LEDCORRECT[this.currentindex - 2].SetActive(true);
            }
            else
            {
                //Debug.Log("test2");
                Debug.LogFormat("[Prime Checker #{0}] <Stage {1}> Not Prime pressed, that is incorrect. Strike.", moduleID, this.currentindex);
                base.GetComponent<KMBombModule>().HandleStrike();
            }
        }
	}

	private void OnRelease()
	{
		//Debug.Log("OnInteractEnded Released");
		if (this.audioRef == null || this.audioRef.StopSound != null)
		{
		}
	}

    //animating method, not originally here
    private IEnumerator animateButton(KMSelectable button)
    {
        animating = true;
        int movement = 0;
        while (movement != 10)
        {
            yield return new WaitForSeconds(0.0001f);
            button.transform.localPosition = button.transform.localPosition + Vector3.up * -0.001f;
            movement++;
        }
        movement = 0;
        while (movement != 10)
        {
            yield return new WaitForSeconds(0.0001f);
            button.transform.localPosition = button.transform.localPosition + Vector3.up * 0.001f;
            movement++;
        }
        StopCoroutine("animateButton");
        animating = false;
    }

    public KMSelectable[] buttons;

	public GameObject[] LEDCORRECT;

	public GameObject[] LEDOFF;

	private KMAudio.KMAudioRef audioRef;

	public int currentindex;

	public bool iscurrentprime;

	public TextMesh maintext;

	private string primenumbers = "\r\n2\r\n3\r\n5\r\n7\r\n11\r\n13\r\n17\r\n19\r\n23\r\n29\r\n31\r\n37\r\n41\r\n43\r\n47\r\n53\r\n59\r\n61\r\n67\r\n71\r\n73\r\n79\r\n83\r\n89\r\n97\r\n101\r\n103\r\n107\r\n109\r\n113\r\n127\r\n131\r\n137\r\n139\r\n149\r\n151\r\n157\r\n163\r\n167\r\n173\r\n179\r\n181\r\n191\r\n193\r\n197\r\n199\r\n211\r\n223\r\n227\r\n229\r\n233\r\n239\r\n241\r\n251\r\n257\r\n263\r\n269\r\n271\r\n277\r\n281\r\n283\r\n293\r\n307\r\n311\r\n313\r\n317\r\n331\r\n337\r\n347\r\n349\r\n353\r\n359\r\n367\r\n373\r\n379\r\n383\r\n389\r\n397\r\n401\r\n409\r\n419\r\n421\r\n431\r\n433\r\n439\r\n443\r\n449\r\n457\r\n461\r\n463\r\n467\r\n479\r\n487\r\n491\r\n499\r\n503\r\n509\r\n521\r\n523\r\n541\r\n547\r\n557\r\n563\r\n569\r\n571\r\n577\r\n587\r\n593\r\n599\r\n601\r\n607\r\n613\r\n617\r\n619\r\n631\r\n641\r\n643\r\n647\r\n653\r\n659\r\n661\r\n673\r\n677\r\n683\r\n691\r\n701\r\n709\r\n719\r\n727\r\n733\r\n739\r\n743\r\n751\r\n757\r\n761\r\n769\r\n773\r\n787\r\n797\r\n809\r\n811\r\n821\r\n823\r\n827\r\n829\r\n839\r\n853\r\n857\r\n859\r\n863\r\n877\r\n881\r\n883\r\n887\r\n907\r\n911\r\n919\r\n929\r\n937\r\n941\r\n947\r\n953\r\n967\r\n971\r\n977\r\n983\r\n991\r\n997\r\n    ";

	private string nonprimenumbers = "\r\n1\r\n9\r\n21\r\n27\r\n33\r\n39\r\n49\r\n51\r\n57\r\n63\r\n69\r\n77\r\n81\r\n87\r\n91\r\n93\r\n99\r\n111\r\n117\r\n119\r\n121\r\n123\r\n129\r\n133\r\n141\r\n143\r\n147\r\n153\r\n159\r\n161\r\n169\r\n171\r\n177\r\n183\r\n187\r\n189\r\n201\r\n203\r\n207\r\n209\r\n213\r\n217\r\n219\r\n221\r\n231\r\n237\r\n243\r\n247\r\n249\r\n253\r\n259\r\n261\r\n267\r\n273\r\n279\r\n287\r\n289\r\n291\r\n297\r\n299\r\n301\r\n303\r\n309\r\n319\r\n321\r\n323\r\n327\r\n329\r\n333\r\n339\r\n341\r\n343\r\n351\r\n357\r\n361\r\n363\r\n369\r\n371\r\n377\r\n381\r\n387\r\n391\r\n393\r\n399\r\n403\r\n407\r\n411\r\n413\r\n417\r\n423\r\n427\r\n429\r\n437\r\n441\r\n447\r\n451\r\n453\r\n459\r\n469\r\n471\r\n473\r\n477\r\n481\r\n483\r\n489\r\n493\r\n497\r\n501\r\n507\r\n511\r\n513\r\n517\r\n519\r\n527\r\n529\r\n531\r\n533\r\n537\r\n539\r\n543\r\n549\r\n551\r\n553\r\n559\r\n561\r\n567\r\n573\r\n579\r\n581\r\n583\r\n589\r\n591\r\n597\r\n603\r\n609\r\n611\r\n621\r\n623\r\n627\r\n629\r\n633\r\n637\r\n639\r\n649\r\n651\r\n657\r\n663\r\n667\r\n669\r\n671\r\n679\r\n681\r\n687\r\n689\r\n693\r\n697\r\n699\r\n703\r\n707\r\n711\r\n713\r\n717\r\n721\r\n723\r\n729\r\n731\r\n737\r\n741\r\n747\r\n749\r\n753\r\n759\r\n763\r\n767\r\n771\r\n777\r\n779\r\n781\r\n783\r\n789\r\n791\r\n793\r\n799\r\n801\r\n803\r\n807\r\n813\r\n817\r\n819\r\n831\r\n833\r\n837\r\n841\r\n843\r\n847\r\n849\r\n851\r\n861\r\n867\r\n869\r\n871\r\n873\r\n879\r\n889\r\n891\r\n893\r\n897\r\n899\r\n901\r\n903\r\n909\r\n913\r\n917\r\n921\r\n923\r\n927\r\n931\r\n933\r\n939\r\n943\r\n949\r\n951\r\n957\r\n959\r\n961\r\n963\r\n969\r\n973\r\n979\r\n981\r\n987\r\n989\r\n993\r\n999\r\n";

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} prime [Presses the prime button] | !{0} notprime [Presses the not prime button]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*prime\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            buttons[0].OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*notprime\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            buttons[1].OnInteract();
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*not prime\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            buttons[1].OnInteract();
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        for(int i = 0; i < 3; i++)
        {
            if (this.iscurrentprime == true)
            {
                yield return ProcessTwitchCommand("prime");
            }
            else if (this.iscurrentprime == false)
            {
                yield return ProcessTwitchCommand("notprime");
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}