﻿using LUSSISADTeam10Web.API;
using LUSSISADTeam10Web.Constants;
using LUSSISADTeam10Web.Models;
using LUSSISADTeam10Web.Models.APIModels;
using LUSSISADTeam10Web.Models.Clerk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Excel = Microsoft.Office.Interop.Excel;

namespace LUSSISADTeam10Web.Controllers
{
    [Authorize(Roles = "Clerk")]
    public class ClerkController : Controller
    {
        // GET: Clerk
        public ActionResult Index()
        {
            return View("ClerkDashboard");
        }

        // Start AM

        public ActionResult ShowActiveSupplierlist()
        {
            string token = GetToken();
            UserModel um = GetUser();
           List< SupplierModel> sm = new List<SupplierModel>();

            

            try
            {
                sm = APISupplier.GetSupplierByStatus(ConSupplier.Active.ACTIVE, token, out string error);

                return View(sm);
             
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }

        }
        public ActionResult ShowDeActiveSupplierlist()
        {
            string token = GetToken();
            UserModel um = GetUser();
            List<SupplierModel> sm = new List<SupplierModel>();



            try
            {
                sm = APISupplier.GetSupplierByStatus(ConSupplier.Active.INACTIVE, token, out string error);

                return View(sm);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }

        }


        public ActionResult SupllierDetails(int id)
        {
            string token = GetToken();
            UserModel um = GetUser();

            SupplierModel sm = new SupplierModel();
            List<SupplierItemModel> smlist = new List<SupplierItemModel>();


            try
            {
                sm = APISupplier.GetSupplierById(id, token, out string supperror);
                ViewBag.suppliername = sm.SupName;
                smlist = APISupplier.GetItemsBySupplierId(id, token, out string error);

                return View(smlist);

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }




        }
        public ActionResult DeActive(int id)
        {

            string token = GetToken();
            UserModel um = GetUser();

            SupplierModel sm = new SupplierModel();
            sm = APISupplier.GetSupplierById(id, token, out string superror);

            try
            {

                APISupplier.DeactivateSupplier(sm, token, out string error);


            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }
            return RedirectToAction("ShowDeActiveSupplierlist");
        }


        public ActionResult Active(int id)
        {

            string token = GetToken();
            UserModel um = GetUser();

            SupplierModel sm = new SupplierModel();
            sm = APISupplier.GetSupplierById(id, token, out string superror);

            try
            {

                APISupplier.ActivateSupplier(sm, token, out string error);


            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }
            return RedirectToAction("ShowActiveSupplierlist");
        }
        public ActionResult csvsupplier1()
        {
            return View("csvsupplier1");
        }

        [HttpPost]
        public ActionResult csvsupplier(HttpPostedFileBase excelfile)
        {
            string token = GetToken();
            UserModel um = GetUser();
            try
            {
                if (excelfile == null || excelfile.ContentLength == 0)
                {

                    ViewBag.Error = "Please select a excel file";
                    return View("Index");
                }

                else
                {

                    if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                    {
                        string path = Server.MapPath("~/Content/" + excelfile.FileName);
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        excelfile.SaveAs(path);
                        // read data from excel file
                        Excel.Application application = new Excel.Application();
                        Excel.Workbook workbook = application.Workbooks.Open(path);
                        Excel.Worksheet worksheet = workbook.ActiveSheet;
                        Excel.Range range = worksheet.UsedRange;
                        List<SupplierItemModel> SuppItem = new List<SupplierItemModel>();
                        for (int row = 2; row <= range.Rows.Count; row++)
                        {


                            SupplierItemModel p = new SupplierItemModel();
                            p.SupId = int.Parse(((Excel.Range)range.Cells[row, 1]).Text);
                            p.SupName = ((Excel.Range)range.Cells[row, 2]).Text;
                            p.ItemId = int.Parse(((Excel.Range)range.Cells[row, 3]).Text);
                            p.Description = ((Excel.Range)range.Cells[row, 4]).Text;
                            p.Price = double.Parse(((Excel.Range)range.Cells[row, 5]).Text);
                            p.Uom = ((Excel.Range)range.Cells[row, 6]).Text;
                            p.CategoryName = ((Excel.Range)range.Cells[row, 7]).Text;
                            SuppItem.Add(p);
                        }


                       List <SupplierItemModel> sm =  APISupplier.csvsupplier(token, SuppItem, out string error);
                        workbook.Close();
                        int i = 0;
                        foreach (SupplierItemModel s in sm) {

                            i = s.SupId;
                        }
                        List<SupplierItemModel> sm1 = APISupplier.GetItemsBySupplierId(i, token, out string error1);

                        return View(sm1);
                    }
                    else
                    {


                        ViewBag.Error = "File type is incorrect";
                        return View("Index");
                    }
                }
            }

            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }
        }


        // End AM

        // Start TAZ
        public ActionResult ApproveCollectionPoint(int id)
        {
            string token = GetToken();
            DepartmentCollectionPointModel dcpm = new DepartmentCollectionPointModel();
            ViewBag.DepartmentCollectionPointModel = dcpm;
            ApproveCollectionPointViewModel viewmodel = new ApproveCollectionPointViewModel();
            List<DepartmentCollectionPointModel> st = new List<DepartmentCollectionPointModel>();
          
           
            try
            {
                dcpm = APICollectionPoint.GetDepartmentCollectionPointByDcpid(token, id ,out string error);
                st = APICollectionPoint.GetDepartmentCollectionPointByStatus(token, 1, out error);
                ViewBag.DepartmentCollectionModel = dcpm;
                ViewBag.DepartmentCollectionModel = st;
                viewmodel.CpID =dcpm.DeptCpID;
                viewmodel.DepName = dcpm.DeptName;
                viewmodel.CpName = dcpm.CpName;
                foreach (DepartmentCollectionPointModel p in st) {
                    viewmodel.OldCpName = p.CpName;
                }
                
                


            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult ApproveCollectionPoint(ApproveCollectionPointViewModel viewmodel)
        {
            string token = GetToken();
            UserModel um = GetUser();
            DepartmentCollectionPointModel dcpm = new DepartmentCollectionPointModel();
           

            dcpm = APICollectionPoint.GetDepartmentCollectionPointByDcpid(token, viewmodel.CpID, out string error);

            try
            {
                
                if (!viewmodel.Approve)
                {
                    dcpm = APICollectionPoint.RejectDepartmentCollectionPoint(token, dcpm, out error);

                }

                else if (viewmodel.Approve)
                {
                    dcpm = APICollectionPoint.ConfirmDepartmentCollectionPoint(token, dcpm, out error);

                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }

        }
        //Manage Items
        public ActionResult Manage()
        {
            string token = GetToken();
            List<InventoryModel> invm = new List<InventoryModel>();
            try
            {
                
                invm=APIInventory.GetAllInventories(token, out string error);
                if (error != "")
                {
                    return RedirectToAction("Index", "Error", new { error });
                }
            }
            catch (Exception ex)
            {
                RedirectToAction("Index", "Error", new { error = ex.Message });
            }

            return View(invm);
        }

        //Edit Item
        public ActionResult EditItem(int id = 0)
        {
            string token = GetToken();
            InventoryModel invm = new InventoryModel();
            ItemModel itm = new ItemModel();
            ViewBag.InventoryModel = invm;
            InventoryViewModel viewmodel = new InventoryViewModel();
            invm = APIInventory.GetInventoryByInvid(id, token, out string error);

            try
            {

                ViewBag.InventoryModel = invm;

                viewmodel.CatId = itm.Catid;
                viewmodel.ItemDescription = invm.ItemDescription;
                viewmodel.Stock = invm.Stock;
                viewmodel.ReorderLevel = invm.ReorderLevel;
                viewmodel.ReorderQty = invm.ReorderQty;
                viewmodel.CategoryName = invm.CategoryName;
                viewmodel.Itemid = invm.Itemid;
                viewmodel.Invid = invm.Invid;
                viewmodel.UOM = invm.UOM;

            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new
                {
                    error = ex.Message
                });
            }
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult EditItem(InventoryViewModel viewmodel)
        {

            string token = GetToken();
            InventoryModel invm = new InventoryModel();
            ItemModel it = new ItemModel();
            CategoryModel c = new CategoryModel();


            invm = APIInventory.GetInventoryByInvid(viewmodel.Invid, token, out string error);
            it = APIItem.GetItemByItemID(viewmodel.Itemid, token, out error);
            c = APICategory.GetCategoryByCatID(token, it.Catid, out error);

            c.Name = viewmodel.CategoryName;
            invm.Invid = viewmodel.Invid;
            invm.Itemid = viewmodel.Itemid;
            invm.Stock = viewmodel.Stock;
            invm.ReorderLevel = viewmodel.ReorderLevel;
            invm.ReorderQty = viewmodel.ReorderQty;
            it.Description = viewmodel.ItemDescription;
            it.Uom = viewmodel.UOM;


            try
            {
                invm = APIInventory.UpdateInventory(token, invm, out error);
                it = APIItem.UpdateItem(token, it, out error);
                c = APICategory.UpdateCategory(token, c, out error);
                return RedirectToAction("Manage");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }

        }
        /////////

        public ActionResult SearchByTransDate(DateTime? startdate,DateTime? enddate)

        {
            string token = GetToken();
            InventoryTransactionViewModel viewmodel = new InventoryTransactionViewModel();
            List<InventoryTransactionModel> intlm = new List<InventoryTransactionModel>();
            ItemModel item = new ItemModel();
            try
            {
                if (startdate == null)
                    startdate = new DateTime(1900, 01, 01);
                if (enddate == null)
                    enddate = new DateTime(2900, 01, 01);
                intlm =APIInventoryTranscation.GetInventoryTransactionByTransDate((DateTime)startdate, (DateTime)enddate, token, out string error);
                viewmodel.InvTrans = new List<InventoryTransactionResultViewModel>();
                
                foreach (InventoryTransactionModel i in intlm)
                {
                    
                   item= APIItem.GetItemByItemID(i.ItemID, token, out error);
                    var result = new InventoryTransactionResultViewModel();         
                    result.ItemID = i.ItemID;
                    result.Description = i.Description;
                    result.UOM = i.UOM;
                    result.Qty = i.Qty;
                    result.Date=i.TransDate;
                    viewmodel.InvTrans.Add(result);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }
            return View(viewmodel);
        }
        public ActionResult ItemTran(DateTime? startdate, DateTime? enddate, int id = 0)
        {
            string token = GetToken();
            InventoryTransactionModel invm = new InventoryTransactionModel();
            ViewBag.InventoryModel = invm;
            InventoryTransactionViewModel viewmodel = new InventoryTransactionViewModel();
           
            List<InventoryTransactionModel> intlm = new List<InventoryTransactionModel>();

            try
            {
                if (startdate == null || enddate == null)
                {
                    intlm = APIInventoryTranscation.GetInventoryTransactionByItemID(id, token, out string error);
                }
                else
                {
                    intlm = APIInventoryTranscation.GetInventoryTransactionByTransDate((DateTime)startdate, (DateTime)enddate, token, out string error);
                    intlm = intlm.Where(p => p.ItemID == id).ToList();
                }
                viewmodel.InvTrans = new List<InventoryTransactionResultViewModel>();

                foreach (InventoryTransactionModel i in intlm)
                {

                    var result = new InventoryTransactionResultViewModel();
                    result.ItemID = i.ItemID;
                    result.Description = i.Description;
                    result.UOM = i.UOM;
                    result.Qty = i.Qty;
                    result.Date = i.TransDate;
                    result.Remark= i.Remark;
                    result.Transtype = i.TransType;
                    viewmodel.InvTrans.Add(result);
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { error = ex.Message });
            }
            return View(viewmodel);
        }
        //END TAZ

        //Start Mahsu

        //Get All InventoryCheckViewModel
        public InventoryCheckViewModel GetInvtCheckVM()
        {
            string token = GetToken();
            UserModel user = GetUser();
            List<InventoryModel> ivmlist = new List<InventoryModel>();
            List<Inventory> ivclist = new List<Inventory>();

            InventoryCheckViewModel invcvm = new InventoryCheckViewModel();

            ivmlist = APIInventory.GetAllInventories(token, out string error);

            foreach (InventoryModel invent in ivmlist)
            {
                CategoryModel cat = APICategory.GetCategoryByItemID(token, invent.Itemid, out error);
                ItemModel item = APIItem.GetItemByItemID(invent.Itemid, token, out error);
                Inventory ivc = new Inventory(invent.Invid, cat.Name, invent.Itemid, invent.ItemDescription, invent.Stock, item.Uom, cat.Shelflocation, cat.Shelflevel);
                ivclist.Add(ivc);
            }

            invcvm.Invs = ivclist;
            invcvm.InvIDs = new List<int>();

            return invcvm;
        }
        //Get Inventory by inventoryID
        public List<Inventory> GetSelectedInventory(List<int> i)
        {
            InventoryCheckViewModel ivcvm = GetInvtCheckVM();
            List<Inventory> ivm = ivcvm.Invs;
            List<Inventory> dis = new List<Inventory>();
            foreach (Inventory iv in ivm)
            {
                foreach (int a in i)
                {
                    if (iv.InventoryId == a)
                        dis.Add(iv);
                }
            }
            return dis;
        }
        //Display Awaiting Approval Adjustments     //Display All Inventories
        public ActionResult Inventory()
        {
            string token = GetToken();
            UserModel user = GetUser();

            List<AdjustmentModel> adj = new List<AdjustmentModel>();
            List<AdjustmentDetailModel> adjdetail = new List<AdjustmentDetailModel>();

            InventoryCheckViewModel invcvm = new InventoryCheckViewModel();
            List<Inventory> inv = new List<Inventory>();
            try
            {
                invcvm = GetInvtCheckVM();
               
                adj = APIAdjustment.GetAdjustmentByStatus(token, ConAdjustment.Active.PENDING, out string error);
                
                foreach(AdjustmentModel ad in adj)
                {                  
                    foreach(AdjustmentDetailModel adjd in ad.Adjds)
                    {
                        foreach(Inventory i in invcvm.Invs)
                        {
                            if (adjd.Itemid == i.ItemID)
                            {
                                adjd.Stock = i.Stock;
                                adjd.Adjustedqty = adjd.Adjustedqty + (int)adjd.Stock;
                            }

                            adjd.IssueDate = ((DateTime)ad.Issueddate);                           
                            
                        }
                        adjdetail.Add(adjd);
                    }
                }
                ViewBag.AdjustmentDetailModel = adjdetail;
                
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }
            return View(invcvm);
        }
       //Get All checked Inventories
        [HttpPost]
        public ActionResult Inventory(List<int> InvID)
            {
            List<Inventory> dis = GetSelectedInventory(InvID);
            TempData["discrepancy"] = dis; 
            return RedirectToAction("Adjustment");
        }
        public ActionResult Adjustment()
        {
            List<Inventory> dis = TempData["discrepancy"] as List<Inventory>;
            InventoryCheckViewModel ivcvm = new InventoryCheckViewModel();
            try
            {
                ivcvm.Invs = dis;
                ivcvm.InvIDs = new List<int>();
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

            return View(ivcvm);
        }
        [HttpPost]
        public ActionResult Adjustment(List<int> InvID, List<int> Current, List<string>Reason)
        {
            string token = GetToken();
            UserModel user = GetUser();
            List<Inventory> invent = GetSelectedInventory(InvID);
            AdjustmentModel adjust = new AdjustmentModel();
            try
            {
                for (int i = 0; i < InvID.Count; i++)
                {
                    foreach (Inventory inv in invent)
                    {
                        if (InvID[i] == inv.InventoryId)
                        {
                            inv.Current = Current[i];
                            AdjustmentDetailModel adjd = new AdjustmentDetailModel(inv.ItemID, (inv.Current - (int)inv.Stock), Reason[i]);
                            adjust.Adjds.Add(adjd);
                        }
                    }
                }
                adjust.Issueddate = DateTime.Now.Date;
                adjust.Raisedby = user.Userid;

                adjust = APIAdjustment.CreateAdjustment(token, adjust, out string error);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error", new { error = e.Message });
            }

            return RedirectToAction("Inventory");
        }


        // End MaHus

        // Start ZMH

        public ActionResult Requisition()
        {
            string token = GetToken();
            UserModel um = GetUser();
            string error = "";

            List<RequisitionModel> reqms = new List<RequisitionModel>();

            reqms = APIRequisition.GetRequisitionByStatus(ConRequisition.Status.APPROVED, token, out error);

            ViewBag.Requisitions = reqms;

            return View();
        }

        public ActionResult RequisitionDetail(int id)
        {
            string token = GetToken();
            UserModel um = GetUser();
            string error = "";
            RequisitionModel reqm = new RequisitionModel();
            reqm = APIRequisition.GetRequisitionByReqid(id, token, out error);
            ViewBag.Requisition = reqm;
            ProcessRequisitionViewModel vm = new ProcessRequisitionViewModel
            {
                ReqID = reqm.Reqid
            };
            vm.ReqItems = new List<ReqItem>();

            foreach (RequisitionDetailsModel rd in reqm.Requisitiondetails)
            {
                ReqItem ri = new ReqItem
                {
                    ItemID = rd.Itemid,
                    ItemName = rd.Itemname,
                    CategoryName = rd.CategoryName,
                    Qty = rd.Qty,
                    Stock = rd.Stock,
                    UOM = rd.UOM
                };
                vm.ReqItems.Add(ri);
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult RequisitionDetail(ProcessRequisitionViewModel viewmodel, List<int> itemids, List<int> ApproveQtys)
        {
            int count = 0;
            string error = "";
            string token = GetToken();
            UserModel um = GetUser();
            bool IsNeededOutstanding = false;
            OutstandingReqModel outr = new OutstandingReqModel();


            RequisitionModel reqm = new RequisitionModel();

            reqm = APIRequisition.GetRequisitionByReqid(viewmodel.ReqID, token, out error);
            List<RequisitionDetailsModel> reqdms = reqm.Requisitiondetails;
            viewmodel.ReqItems = new List<ReqItem>();
            foreach (int itemid in itemids)
            {
                ReqItem ri = new ReqItem();
                RequisitionDetailsModel reqdm = reqdms.Where(p => p.Itemid == itemid).FirstOrDefault();
                ri.ItemID = itemid;
                ri.ApproveQty = ApproveQtys[count];
                ri.Qty = reqdm.Qty;
                ri.Stock = reqdm.Stock;
                ri.UOM = reqdm.UOM;
                ri.ItemName = reqdm.Itemname;
                ri.CategoryName = reqdm.CategoryName;
                if((ri.Qty - ri.ApproveQty) > 0)
                {
                    IsNeededOutstanding = true;
                }
                viewmodel.ReqItems.Add(ri);
                count++;
            }

            DisbursementModel dis = new DisbursementModel();
            dis.Reqid = viewmodel.ReqID;
            dis.Ackby = um.Userid;
            dis = APIDisbursement.Createdisbursement(dis, token, out error);

            if (IsNeededOutstanding)
            {
                outr.ReqId = viewmodel.ReqID;
                outr.Reason = "Not Enough Stock";
                outr.Status = ConOutstandingsRequisition.Status.PENDING;
                outr = APIOutstandingReq.CreateOutReq(outr, token, out error);
            }

            foreach(ReqItem ri in viewmodel.ReqItems)
            {
                    DisbursementDetailsModel disdm = new DisbursementDetailsModel();
                    disdm.Disid = dis.Disid;
                    disdm.Itemid = ri.ItemID;
                    disdm.Qty = ri.ApproveQty;
                    disdm = APIDisbursement.CreateDisbursementDetails(disdm, token, out error);
                if (ri.Qty > ri.ApproveQty)
                {
                    OutstandingReqDetailModel outreq = new OutstandingReqDetailModel();
                    outreq.OutReqId = outr.OutReqId;
                    outreq.ItemId = ri.ItemID;
                    outreq.Qty = ri.ApproveQty - ri.Qty;
                    outreq = APIOutstandingReq.CreateOutReqDetail(outreq, token, out error);
                }
            }
            reqm = APIRequisition.UpdateRequisitionStatusToPending(reqm, token, out error);
            
            return View("Requisition");
        }

        public ActionResult Outstanding()
        {
            string token = GetToken();
            UserModel um = GetUser();
            string error = "";

            List<OutstandingReqModel> outrm = new List<OutstandingReqModel>();

            outrm = APIOutstandingReq.GetAllOutReqs(token, out error);
            outrm.Where(p => p.Status == ConOutstandingsRequisition.Status.PENDING).ToList();

            ViewBag.Outstandings = outrm;

            return View();
        }

        public ActionResult UpdateToPreparing()
        {
            string token = GetToken();
            UserModel um = GetUser();
            string error = "";

            List<RequisitionWithDisbursementModel> reqdisms = new List<RequisitionWithDisbursementModel>();

            reqdisms = APIRequisition.UpdateAllRequistionRequestStatusToPreparing(token, out error);

            return RedirectToAction("DisbursementLists");
        }

        public ActionResult DisbursementLists()
        {
            string token = GetToken();
            UserModel um = GetUser();
            string error = "";
            ViewBag.ShowItems = false;


            List<RequisitionWithDisbursementModel> reqdisms = new List<RequisitionWithDisbursementModel>();
            reqdisms = APIRequisition.GetRequisitionWithPreparingStatus(token, out error);

            List<string> CollectionPointNames = new List<string>();
            CollectionPointNames = reqdisms.Select(x => x.Cpname).Distinct().ToList();
            ViewBag.CollectionPointNames = CollectionPointNames;
            if (CollectionPointNames.Count > 0)
            {
                ViewBag.ShowItems = true;
            }

            return View(reqdisms);
        }

        public ActionResult ItemDelivered(int id)
        {
            string error = "";
            string token = GetToken();
            UserModel um = GetUser();

            RequisitionModel req = new RequisitionModel();
            req = APIRequisition.GetRequisitionByReqid(id, token, out error);

            if(req.Status != ConRequisition.Status.PREPARING)
            {
                RedirectToAction("DisbursementLists");
            }

            req.Status = ConRequisition.Status.DELIVERED;
            req = APIRequisition.UpdateRequisition(req, token, out error);
            
            // add notification here

            return RedirectToAction("DisbursementDetail", new { id = req.Reqid });
        }

        public ActionResult DisbursementDetail(int id)
        {
            string error = "";
            string token = GetToken();
            UserModel um = GetUser();


            RequisitionWithDisbursementModel req = new RequisitionWithDisbursementModel();

            req = APIRequisition.GetRequisitionWithDisbursementByReqID(id, token, out error);

            if (req.Status != ConRequisition.Status.DELIVERED)
            {
                RedirectToAction("DisbursementLists");
            }

            return View(req);

        }

        // End ZMH

        // Start Phyo2

        public ActionResult GetOrderRecommndation()
        {
            string token = GetToken();
            UserModel um = GetUser();

            List<InventoryDetailModel> inendetail = new List<InventoryDetailModel>();

            try
            {
                inendetail = APIInventory.GetAllInventoryDetails(token, out string error);

                if (error != "")
                {
                    return RedirectToAction("Index", "Error", new { error });
                }
            }
            catch (Exception ex)
            {
                RedirectToAction("Index", "Error", new { error = ex.Message });
            }

            return View(inendetail);
        }

        public ActionResult StationaryRetrievalForm()
        {
            string token = GetToken();
            UserModel um = GetUser();

            List<OutstandingItemModel> inendetail = new List<OutstandingItemModel>();
            List<BreakdownByDepartmentModel> bkm = new List<BreakdownByDepartmentModel>();

            List<ShowBD> bkmd = new List<ShowBD>();
            try
            {
                inendetail = APIDisbursement.GetRetriveItemListforClerk(token, out string error);

                bkm = APIDisbursement.GetBreakDown(token, out string errors);
                
                
            }




            catch(Exception ex)
            {
                var mes = ex.Message;
            }

            return View(bkm);
        }


        // End Phyo2

        #region Utilities
        public string GetToken()
        {
            string token = "";
            token = (string)Session["token"];
            if (string.IsNullOrEmpty(token))
            {
                token = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                Session["token"] = token;
                UserModel um = APIAccount.GetUserProfile(token, out string error);
                Session["user"] = um;
                Session["role"] = um.Role;
            }
            return token;
        }
        public UserModel GetUser()
        {
            UserModel um = (UserModel)Session["user"];
            if(um == null)
            {
                GetToken();
                um = (UserModel)Session["user"];
            }
            return um;
        }
        #endregion
    }
}