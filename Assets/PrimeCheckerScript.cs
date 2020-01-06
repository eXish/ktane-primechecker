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
        if(moduleSolved != true)
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
        if(moduleSolved != true)
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
    }

    public KMSelectable[] buttons;

	public GameObject[] LEDCORRECT;

	public GameObject[] LEDOFF;

	private KMAudio.KMAudioRef audioRef;

	public int currentindex;

	public bool iscurrentprime;

	public TextMesh maintext;

	private string primenumbers = "\n2\n3\n5\n7\n11\n13\n17\n19\n23\n29\n31\n37\n41\n43\n47\n53\n59\n61\n67\n71\n73\n79\n83\n89\n97\n101\n103\n107\n109\n113\n127\n131\n137\n139\n149\n151\n157\n163\n167\n173\n179\n181\n191\n193\n197\n199\n211\n223\n227\n229\n233\n239\n241\n251\n257\n263\n269\n271\n277\n281\n283\n293\n307\n311\n313\n317\n331\n337\n347\n349\n353\n359\n367\n373\n379\n383\n389\n397\n401\n409\n419\n421\n431\n433\n439\n443\n449\n457\n461\n463\n467\n479\n487\n491\n499\n503\n509\n521\n523\n541\n547\n557\n563\n569\n571\n577\n587\n593\n599\n601\n607\n613\n617\n619\n631\n641\n643\n647\n653\n659\n661\n673\n677\n683\n691\n701\n709\n719\n727\n733\n739\n743\n751\n757\n761\n769\n773\n787\n797\n809\n811\n821\n823\n827\n829\n839\n853\n857\n859\n863\n877\n881\n883\n887\n907\n911\n919\n929\n937\n941\n947\n953\n967\n971\n977\n983\n991\n997";

	private string nonprimenumbers = "\n1\n9\n21\n27\n33\n39\n49\n51\n57\n63\n69\n77\n81\n87\n91\n93\n99\n111\n117\n119\n121\n123\n129\n133\n141\n143\n147\n153\n159\n161\n169\n171\n177\n183\n187\n189\n201\n203\n207\n209\n213\n217\n219\n221\n231\n237\n243\n247\n249\n253\n259\n261\n267\n273\n279\n287\n289\n291\n297\n299\n301\n303\n309\n319\n321\n323\n327\n329\n333\n339\n341\n343\n351\n357\n361\n363\n369\n371\n377\n381\n387\n391\n393\n399\n403\n407\n411\n413\n417\n423\n427\n429\n437\n441\n447\n451\n453\n459\n469\n471\n473\n477\n481\n483\n489\n493\n497\n501\n507\n511\n513\n517\n519\n527\n529\n531\n533\n537\n539\n543\n549\n551\n553\n559\n561\n567\n573\n579\n581\n583\n589\n591\n597\n603\n609\n611\n621\n623\n627\n629\n633\n637\n639\n649\n651\n657\n663\n667\n669\n671\n679\n681\n687\n689\n693\n697\n699\n703\n707\n711\n713\n717\n721\n723\n729\n731\n737\n741\n747\n749\n753\n759\n763\n767\n771\n777\n779\n781\n783\n789\n791\n793\n799\n801\n803\n807\n813\n817\n819\n831\n833\n837\n841\n843\n847\n849\n851\n861\n867\n869\n871\n873\n879\n889\n891\n893\n897\n899\n901\n903\n909\n913\n917\n921\n923\n927\n931\n933\n939\n943\n949\n951\n957\n959\n961\n963\n969\n973\n979\n981\n987\n989\n993\n999";

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
        if (Regex.IsMatch(command, @"^\s*not ?prime\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            buttons[1].OnInteract();
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        for(int i = currentindex; i < 4; i++)
        {
            if (this.iscurrentprime == true)
            {
                yield return ProcessTwitchCommand("prime");
            }
            else if (this.iscurrentprime == false)
            {
                yield return ProcessTwitchCommand("notprime");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}