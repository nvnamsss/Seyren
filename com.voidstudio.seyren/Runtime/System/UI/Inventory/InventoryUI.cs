// public class InventoryUI : MonoBehaviour
// {
//     // Start is called before the first frame update

//     public Transform itemParent;

//     public GameObject inventoryUI;

//     public Button nextButton;

//     public Button previousButton;

//     public int currPage = 1;

//     public Text currPageText;

//     public Text totalPageText;

//     public int totalPage = 1; 

//     InventoryManager inventory;

//     InventorySlot[] slots;
//     void Start()
//     {
//         inventory = InventoryManager.instance;
//         inventory.onItemChangedCallBack += updateUI;

//         checkButton();

//         slots = itemParent.GetComponentsInChildren<InventorySlot>();

//         updateUI();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(Input.GetKeyDown("i")){
//             inventoryUI.SetActive(!inventoryUI.activeSelf);
//         }   
//     }

//     void updateUI(){
//         Debug.Log((float)inventory.items.Count);
//         float temp = ((float)inventory.items.Count/20);
//         Debug.Log(temp.ToString());
        
//         totalPage = (int)Math.Ceiling(temp-0.01f);

//         if (totalPage<1) 
//             totalPage = 1;

//         totalPageText.text = totalPage.ToString();
//         currPageText.text = currPage.ToString();

//         checkButton();

//         for (int i = 0 ; i< slots.Length;i++){
//             if(i< (inventory.items.Count - (20*(currPage-1)))){
//                 slots[i].addItemToSlot(inventory.items[i+((currPage - 1)* 20)], inventory.itemCounts[i+((currPage - 1)* 20)]);
//             }
//             else{
//                 slots[i].clearItemFromSlot();
//             }
//         }
//     }

//     public void nextPage(){
//         currPage++;
//         updateUI();
//     }

//     public void previousPage(){
//         currPage--;
//         updateUI();
//     }

//     void checkButton(){
//         if (currPage == 1){
//             previousButton.interactable = false;
//         }else{
//             previousButton.interactable = true;
//         }
//         if (currPage == totalPage){
//             nextButton.interactable = false;
//         }
//         else{
//             nextButton.interactable = true;
//         }
//     }
// }
