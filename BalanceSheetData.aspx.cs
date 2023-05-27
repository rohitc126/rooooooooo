using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class FAMS_Master_BalanceSheetData : System.Web.UI.Page
{
    BAL_EmployeeLevelAccess EmployeeLevelBAL = new BAL_EmployeeLevelAccess();
    Message msg = new Message();
    BAL_FA_LedgerMapping Notes = new BAL_FA_LedgerMapping();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fill_company();
            fill_Finance_year();
            btnSave.Visible = false;
            gridheader.Visible = false;
           
        }
    }


    protected void fill_company()
    {
        DDLCompanyName.Items.Clear();
        DataTable dtCompany = EmployeeLevelBAL.LoadEmployeeCompanyAccess(Session["LogIn_Code"].ToString());
        if (dtCompany.Rows.Count > 0)
        {
            DDLCompanyName.DataSource = dtCompany;
            DDLCompanyName.DataTextField = "Comp_Name";
            DDLCompanyName.DataValueField = "Comp_Code";
            DDLCompanyName.DataBind();
            DDLCompanyName.Items.Insert(0, new ListItem("Select Company Name", "0"));
        }
    }



    protected void fill_Finance_year()
    {
        DateTime dtm = new DateTime();
        dtm = Convert.ToDateTime(DateTime.Now);
        ArrayList Year = new ArrayList();
      
        Year.Add(Convert.ToString(dtm.Year - 3) + "-" + Convert.ToString(dtm.Year - 2));
        Year.Add(Convert.ToString(dtm.Year-2) + "-" + Convert.ToString(dtm.Year-1));
        Year.Add(Convert.ToString(dtm.Year-1) + "-" + Convert.ToString(dtm.Year));
        Year.Add(Convert.ToString(dtm.Year) + "-" + Convert.ToString(dtm.Year + 1));
        //Year.Add(Convert.ToString(dtm.Year + 1) + "-" + Convert.ToString(dtm.Year + 2));
  
        ddlFinanceYear.DataSource = Year;
        ddlFinanceYear.DataBind();
        ddlFinanceYear.Items.Insert(0, new ListItem("Select Finance Year", "0"));
       
    }


    protected void btnSave_Click1(object sender, EventArgs e)
    {
        try
        {
            DataTable dtjv = new DataTable();
           
            ErrorContainer.Visible = true;

            dtjv.Columns.Add("Comp_Code", typeof(string));
            dtjv.Columns.Add("Note", typeof(string));
            dtjv.Columns.Add("Items", typeof(string));
            dtjv.Columns.Add("Qty", typeof(int));
            dtjv.Columns.Add("Rate", typeof(decimal));
            dtjv.Columns.Add("bs_Id", typeof(int));
            DataRow dr = null;

            DataTable dtjv2 = new DataTable();
            dtjv2.Columns.Add("Comp_Code", typeof(string));
            dtjv2.Columns.Add("Note", typeof(string));
            dtjv2.Columns.Add("Items", typeof(string));
            dtjv2.Columns.Add("Qty", typeof(int));
            dtjv2.Columns.Add("Rate", typeof(decimal));
            dtjv2.Columns.Add("bs_Id", typeof(int));

            DataRow dr1 = null;
          
         
         
            foreach (GridViewRow gvr in GVBalance.Rows)
            {
              //  Label lblBS_ID = (Label)gvr.FindControl("lblBS_ID"); 
                Label lblNote = (Label)gvr.FindControl("lblNote");
                Label lblItem = (Label)gvr.FindControl("lblItem");
                TextBox txtQTY = (TextBox)gvr.FindControl("txtQTY");
                TextBox txtRATE = (TextBox)gvr.FindControl("txtRATE");


                HiddenField hdnQTY = (HiddenField)gvr.FindControl("hdnQTY");
                HiddenField hdnRATE = (HiddenField)gvr.FindControl("hdnRATE");

                HiddenField bs_Id = (HiddenField)gvr.FindControl("hdnbsid");
                HiddenField hdnAction = (HiddenField)gvr.FindControl("hdnAction");


                if (txtQTY.Text == string.Empty || txtQTY.Text == "")                   
                {
                    txtQTY.Text = "0";
                }
                if (txtRATE.Text == string.Empty || txtRATE.Text == "")
                {
                    txtRATE.Text = "0";
                }


                if ((Convert.ToInt32(txtQTY.Text) != Convert.ToInt32(hdnQTY.Value) || Convert.ToDecimal(txtRATE.Text) != Convert.ToDecimal(hdnRATE.Value)) && Convert.ToInt32(bs_Id.Value) == Convert.ToInt32(  0))
                {
                    if ((Convert.ToInt32(txtQTY.Text) > 0) && (Convert.ToDecimal(txtRATE.Text) > 0))
                    {

                        dr = dtjv.NewRow();

                        dr["Comp_Code"] = Convert.ToString(DDLCompanyName.SelectedValue);
                        // dr["bs_Id"] = Convert.ToInt32(lblBS_ID.Text);
                        dr["NOTE"] = Convert.ToString(lblNote.Text);
                        dr["Items"] = Convert.ToString(lblItem.Text);
                        dr["QTY"] = Convert.ToInt32(txtQTY.Text);
                        dr["RATE"] = Convert.ToDecimal(txtRATE.Text);
                        dr["bs_Id"] = Convert.ToInt32(bs_Id.Value);
                        dtjv.Rows.Add(dr);
                    }
                }

                else if ((Convert.ToInt32(txtQTY.Text) != Convert.ToInt32(hdnQTY.Value) ||  Convert.ToDecimal(txtRATE.Text) != Convert.ToDecimal(hdnRATE.Value)) && Convert.ToInt32(bs_Id.Value) >Convert.ToInt32(  0))
                {
                    if ((Convert.ToInt32(txtQTY.Text) > 0) && (Convert.ToDecimal(txtRATE.Text) > 0))
                    {
                        dr1 = dtjv2.NewRow();
                        dr1["Comp_Code"] = Convert.ToString(DDLCompanyName.SelectedValue);
                        // dr1["bs_Id"] = Convert.ToInt32(lblBS_ID.Text);
                        dr1["NOTE"] = Convert.ToString(lblNote.Text);
                        dr1["Items"] = Convert.ToString(lblItem.Text);
                        dr1["QTY"] = Convert.ToInt32(txtQTY.Text);
                        dr1["RATE"] = Convert.ToDecimal(txtRATE.Text);
                        dr1["bs_Id"] = Convert.ToInt32(bs_Id.Value);
                        dtjv2.Rows.Add(dr1);
                    }

                }


                /*

                if (bs_Id.Value == null || bs_Id.Value == "0")
                {
                   
                    dr = dtjv.NewRow();

                    if ((Convert.ToInt32(txtQTY.Text) > 0) && (Convert.ToDecimal(txtRATE.Text) > 0))
                    {
                        dr["Comp_Code"] = Convert.ToString(DDLCompanyName.SelectedValue);
                       // dr["bs_Id"] = Convert.ToInt32(lblBS_ID.Text);
                        dr["NOTE"] = Convert.ToString(lblNote.Text);
                        dr["Items"] = Convert.ToString(lblItem.Text);
                        dr["QTY"] = Convert.ToInt32(txtQTY.Text);
                        dr["RATE"] = Convert.ToDecimal(txtRATE.Text);
                        dr["bs_Id"] = Convert.ToInt32(bs_Id.Value);
                        dtjv.Rows.Add(dr);
                    }
                }

                else 
                {

                    dr1 = dtjv2.NewRow();
                    dr1["Comp_Code"] = Convert.ToString(DDLCompanyName.SelectedValue);
                   // dr1["bs_Id"] = Convert.ToInt32(lblBS_ID.Text);
                    dr1["NOTE"] = Convert.ToString(lblNote.Text);
                    dr1["Items"] = Convert.ToString(lblItem.Text);
                    dr1["QTY"] = Convert.ToInt32(txtQTY.Text);
                    dr1["RATE"] = Convert.ToDecimal(txtRATE.Text);
                    dr1["bs_Id"] = Convert.ToInt32(bs_Id.Value);
                    dtjv2.Rows.Add(dr1);
                }
         */
                }

                    string result = "";
                    if (Convert.ToString(DDLCompanyName.SelectedValue) != "0" && (dtjv.Rows.Count > 0 || dtjv2.Rows.Count > 0))
                    {

                        result = Notes.Update_BS_NOTES(Convert.ToString(DDLCompanyName.SelectedValue), dtjv, dtjv2, Session["Employee_Code"].ToString());
                        if (result == "")
                        {
                            //GVBalance.Visible = false;
                            //DDLCompanyName.ClearSelection();
                            //ddlFinanceYear.ClearSelection();


                            msg.ShowMessage("Record Updation is Successfully done.", null, ErrorContainer, MyMessage, "Success");
                        }

                    
                    //else 
                    //{

                    //    msg.ShowMessage("Record Updation Failed. Please Try Again.!", null, ErrorContainer, MyMessage, "Warning");
                    //}

                    }
                    else
                    {
                        msg.ShowMessage("Please Fill Balance Sheet Details!", null, ErrorContainer, MyMessage, "Warning");

                    }

                    
        }
        catch (Exception ex)
        {
            ErrorContainer.Visible = true;
            msg.ShowMessage(null, ex, ErrorContainer, MyMessage, null);
        }
        
    }

  
    protected void fill_note()
    {
        DataTable dt = Notes.SELECT_BS_NOTES(ddlFinanceYear.SelectedValue, DDLCompanyName.SelectedValue); 
        GVBalance.DataSource = dt;
        GVBalance.DataBind();
        ErrorContainer.Visible = false;
        GVBalance.Visible = true;
        btnSave.Visible = true;
        gridheader.Visible = true;
   
    }



    protected void ddlFinanceYear_SelectedIndexChanged(object sender, EventArgs e)
    {
         fill_note();
      
    }
    protected void GVBalance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField bs_Id = (HiddenField)e.Row.FindControl("hdnbsid");
            HiddenField hdnAction = (HiddenField)e.Row.FindControl("hdnAction");
            if (bs_Id.Value == null || bs_Id.Value == "0")
            {
                hdnAction.Value = "I";
            }
            else 
            {
                hdnAction.Value = "U";
            }

        }*/
    }
}