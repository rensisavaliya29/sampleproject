using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace sampleproject
{
    public partial class electronics : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fnconnect();
                bindgrid();
                BindType();
                //FnBindType(); 
            }
        }

        public void fnconnect()
        {

            con = new SqlConnection(strcon);

            if (con.State != System.Data.ConnectionState.Open)
            {
                con.Open();
                Response.Write("connected to database successfully !");
            }
            else
            {
                Response.Write("connection is already open !");
            }
        }

        //protected void FnBindType()
        //{
        //    ddlType.Items.Add("mobile");
        //    ddlType.Items.Add("laptop");
        //    ddlType.Items.Add("tablet");
        //    ddlType.Items.Insert(0, new ListItem("select"));
        //}

        //protected void fnbrand()
        //{
        //    ddlBrand.Items.Clear(); 
        //    if (ddlType.SelectedValue == "mobile")
        //    {
        //        ddlBrand.Items.Add("MI");
        //        ddlBrand.Items.Add("APPLE");
        //        ddlBrand.Items.Add("SAMSUNG");
        //    }
        //    else if(ddlType.SelectedValue == "laptop")
        //    {
        //        ddlBrand.Items.Add("APPLE");
        //    }
        //    else if(ddlType.SelectedValue == "tablet")
        //    {
        //        ddlBrand.Items.Add("MI");
        //    }

        //    ddlBrand.Items.Insert(0, new ListItem("select"));
        //}

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //fnbrand();
            BindBrand();
        }

        public void bindgrid()
        {
            try
            {

                string query = "select * from tbldevice";
                SqlConnection con = new SqlConnection(strcon);
                SqlDataAdapter sda = new SqlDataAdapter(query, con);

                DataTable ds = new DataTable();
                sda.Fill(ds);

                gvData.DataSource = ds;
                gvData.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("error :" + ex.ToString());
            }

        }

        public void BindType()
        {
            try {
                 

                SqlConnection con = new SqlConnection(strcon);
                con.Open();
            string query = "select * from tblType";
            SqlDataAdapter adpt = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();

            adpt.Fill(dt);
            ddlType.DataSource = dt;
            ddlType.DataTextField = "type";
            ddlType.DataValueField ="t_id";
            ddlType.DataBind();
            con.Close();
                ddlType.Items.Insert(0, new ListItem("select"));

            }
            catch (Exception ex)
            {

                Response.Write("error : " +ex.ToString());
            }
        }

        public void BindBrand()
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                con.Open();
                string query = "SELECT * FROM tblBrand WHERE t_id=@t_id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@t_id", ddlType.SelectedValue);

                SqlDataAdapter ds = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                ds.Fill(dt);
                ddlBrand.DataSource = dt;
                ddlBrand.DataTextField = "BrandName";
                ddlBrand.DataValueField = "b_id";
                ddlBrand.DataBind();

                ddlBrand.Items.Insert(0, new ListItem("select"));

            }
            catch (Exception ex)
            {
                Response.Write("Error: " + ex.ToString());
            }
        }

        protected void btnInsert_Click(object sender, EventArgs e)
        {
            //string did = ddlType.SelectedValue;
            string dbid = ddlBrand.SelectedValue;
            string dmodel = txtModel.Text;
            string ddesc = txtDesc.Text;
            string dprice = txtPrice.Text;
            string dquantity = txtQuantity.Text;
            string dcolors = rblColors.SelectedValue;

            string daccess = string.Join(",", cblAccessories.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value));
            string query = "INSERT INTO tblDevice (b_id,model,description,price,quantity,color,accessories) values (@b_id,@model,@description,@price,@quantity,@color,@accessories)";

            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@b_id", dbid);
            cmd.Parameters.AddWithValue("@model", dmodel);
            cmd.Parameters.AddWithValue("@description", ddesc);
            cmd.Parameters.AddWithValue("@price", dprice);
            cmd.Parameters.AddWithValue("@quantity", dquantity);
            cmd.Parameters.AddWithValue("@color", dcolors);
            cmd.Parameters.AddWithValue("@accessories", daccess);

            con.Open();
            cmd.ExecuteNonQuery();
            bindgrid();
            con.Close();
        }

        public void bindgrid2()
        {
            //try
            //{
            //    fnconnect(); // Ensure connection is initialized
            //    string query = "SELECT d.d_id, d.b_id, b.BrandName, d.model, d.description, d.price, d.quantity, d.color, d.accessories " +
            //                   "FROM tblDevice d " +"JOIN tblBrand b ON d.b_id = b.b_id";

            //    SqlCommand cmd = new SqlCommand(query, con);
            //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //    DataSet ds = new DataSet();
            //    sda.Fill(ds);

            //    gvData.DataSource = ds;
            //    gvData.DataBind();
            //}
            //catch (Exception ex)
            //{
            //    Response.Write("error :" + ex.ToString());
            //}
            
        }


        protected void gvData_SelectedIndexChanged(object sender, EventArgs e)
        {
            

            GridViewRow row = gvData.SelectedRow;

            string d_id = row.Cells[1].Text;
            string b_id = row.Cells[2].Text;
            string model = row.Cells[3].Text;
            string description = row.Cells[4].Text;
            string price = row.Cells[5].Text;
            string quantity = row.Cells[6].Text;
            string color = row.Cells[7].Text;
            string accessories = row.Cells[8].Text;

            //ddlType.SelectedValue = d_id;
            //ddlBrand.SelectedValue = b_id;
            txtModel.Text = model;
            txtDesc.Text = description;
            txtPrice.Text = price;
            txtQuantity.Text = quantity;
            rblColors.SelectedValue = color;
            cblAccessories.SelectedValue = accessories;
            foreach(ListItem item in rblColors.Items)
            {
                item.Selected = color.Contains(item.Value);
            }

            foreach (ListItem item in cblAccessories.Items)
            {
                item.Selected = accessories.Contains(item.Value);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = gvData.SelectedRow;
            int d_id = Convert.ToInt16(row.Cells[1].Text);

            //string t_id = ddlType.SelectedValue;
            //string b_id = ddlBrand.SelectedValue;
            string model = txtModel.Text;  
            string description = txtDesc.Text;
            string price = txtPrice.Text;
            string quantity = txtQuantity.Text;
            string color = rblColors.SelectedValue;
            string accessories = cblAccessories.SelectedValue;

            string query = " UPDATE tblDevice SET model=@model , description=@desc , price=@price , quantity=@quantity , color=@color , accessories=@accessories WHERE d_id=@did";

            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);
            //cmd.Parameters.AddWithValue("@tid",t_id);
            //cmd.Parameters.AddWithValue("@bid", b_id);
            cmd.Parameters.AddWithValue("@model", model);
            cmd.Parameters.AddWithValue("@desc", description);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@color", color);
            cmd.Parameters.AddWithValue("@accessories", accessories);
            cmd.Parameters.AddWithValue("@did", d_id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            bindgrid(); 

            }
            catch (Exception ex)
            {

                Response.Write("error in this code is : "+ex.ToString());
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
           
            GridViewRow row = gvData.SelectedRow;
            int did = Convert.ToInt16(row.Cells[1].Text); 

            string query = "DELETE FROM tblDevice WHERE d_id=@did";

            SqlConnection con = new SqlConnection(strcon);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@did", did);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            bindgrid();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(strcon);
            string filename=Path.GetFileName(fu_upload.PostedFile.FileName);

            fu_upload.SaveAs(Server.MapPath("~/Images/") + filename);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into tbl_image(Image_path) values(@Image),con");

            cmd.Parameters.AddWithValue("@Image", "Images/" + filename);

            cmd.ExecuteNonQuery();
            con.Close();

        }
    }
}

