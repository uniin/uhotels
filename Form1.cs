using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using booking.Properties;

namespace Бронирование_мест_в_отеле
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;
		MySqlCommand cmd;
        string user = "guest";

        object app_id, password, wifi, fridge, free_breakfast, conditioner, shower, tv, bed_type, status, comment, m_comment;

        public Form1()
        {
            InitializeComponent();
            initialSettings();
        }

        public void initialSettings()
        {
            Connection();
            tboxLogin.Text = (string)Settings.Default["username"];

            tabControl.TabPages.Remove(tabApplications);
            tabControl.TabPages.Remove(tabReview);
            tabControl.TabPages.Remove(tabBooking);
            tabControl.TabPages.Remove(tabMyApplications);
            tabControl.TabPages.Remove(tabData);
            tabControl.TabPages.Remove(tabRecovery);

            btnExit.Enabled = false;
            labelUser.Enabled = false;

            if (tBoxCommentR.Text == "")
            { tBoxCommentR.Enabled = false; }
            else { tBoxCommentR.Enabled = true; }

            if (tboxMyComment.Text == "")
            { tboxMyComment.Enabled = false; }
            else { tboxMyComment.Enabled = true; }

            if (tboxCommentMyModer.Text == "")
            { tboxCommentMyModer.Enabled = false; }
            else { tboxCommentMyModer.Enabled = true; }

            if (tboxCommentM.Text == "")
            { tboxCommentM.Enabled = false; }
            else { tboxCommentM.Enabled = true; }
        }

        public async void Connection()
        {
            try
            {
                conn = new MySqlConnection($"{Settings.Default["tConnectionStringT"]}"); // tConnectionString, tConnectionStringP, tConnectionStringT
                cmd = new MySqlCommand("", conn);
                conn.Open();
            }
            catch
            {
                MessageBox.Show("Кажется, у Вас отсутствует подключение к интернету! Попробуйте через некоторое время перезапустить программу.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
			cmd.Parameters.Clear();
			conn.Close();
            await Task.WhenAll();
        }

        private void Moder(object sender, EventArgs e)
        {
            if (moder.Checked == true)
            {
                tabControl.TabPages.Add(tabApplications);
                tabControl.SelectTab(tabApplications);
            }
            else if(user != "guest")
            {
                tabControl.TabPages.Remove(tabApplications);
                tabControl.TabPages.Remove(tabData);
                btnExit.Enabled = true;
            }
        }

        private async void btnReg_Click(object sender, EventArgs e) // +
        {
			try
            {
                await conn.OpenAsync();
                cmd.CommandText = $"SELECT NickName FROM Users WHERE NickName='{tboxLoginReg.Text}'";
				MySqlDataReader CheckReg = (MySqlDataReader)await cmd.ExecuteReaderAsync();
				if (tboxLoginReg.Text == "guest" || tboxLoginReg.Text == "admin")
                {
                    MessageBox.Show("Этот логин зарезервирован администрацией сервиса для служебных целей.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tboxLoginReg.Focus();
                }
				else if (tboxLoginReg.Text == "")
				{
					MessageBox.Show("Кажется, что Вы оставили пустым поле введения логина.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxLoginReg.Focus();
				}
				else if (!Regex.IsMatch(tboxLoginReg.Text, @"^[a-zA-Z0-9_]+$"))
				{
					MessageBox.Show("Кажется, что Вы вводите некорректные данные в поле введения логина.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxLoginReg.Focus();
				}
				else if (!Regex.IsMatch(tboxLoginReg.Text, @"^[a-zA-Z]+$") && Regex.IsMatch(tboxLoginReg.Text, @"^[0-9]+$"))
				{
					MessageBox.Show("Логин обязательно должен содержать хотя-бы одну букву.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxLoginReg.Focus();
				}
				else if (tboxLoginReg.TextLength > 20)
				{
					MessageBox.Show("Кажется, что ввёденный текст в поле введения логина превышает лимит в 30 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxLoginReg.Focus();
				}
				else if (CheckReg.Read())
				{
					MessageBox.Show("Кажется, что пользователь с таким логином уже зарегистрирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxLoginReg.Focus();
				}
				else if (tboxPasswordReg.Text == "")
				{
					MessageBox.Show("Кажется, что Вы оставили пустым поле введения пароля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPasswordReg.Focus();
				}
				else if (!Regex.IsMatch(tboxPasswordReg.Text, @"^[a-zA-Z0-9_]+$"))
				{
					MessageBox.Show("Кажется, что Вы вводите некорректные данные в поле введения пароля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPasswordReg.Focus();
				}
				else if (tboxPasswordReg.TextLength < 10)
				{
					MessageBox.Show("Кажется, что ввёденный текст в поле введения пароля не превышает лимит в 10 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPasswordReg.Focus();
				}
				else if (tboxPasswordReg.TextLength > 30)
				{
					MessageBox.Show("Кажется, что ввёденный текст в поле введения пароля превышает лимит в 30 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPasswordReg.Focus();
				}
				else if (tboxPasswordReg2.Text == "")
                {
                    MessageBox.Show("Кажется, что Вы оставили пустым поле повторного введения пароля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tboxPasswordReg2.Focus();
                }
				else if (!Regex.IsMatch(tboxPasswordReg2.Text, @"^[a-zA-Z0-9_]+$"))
				{
					MessageBox.Show("Кажется, что Вы вводите некорректные данные в поле введения повторного пароля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPasswordReg2.Focus();
				}
				else if (tboxPasswordReg2.TextLength < 10)
				{
					MessageBox.Show("Кажется, что ввёденный текст в поле введения повторного пароля не превышает лимит в 10 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPasswordReg2.Focus();
				}
				else if (tboxPasswordReg2.TextLength > 30)
				{
					MessageBox.Show("Кажется, что ввёденный текст в поле введения повторного пароля превышает лимит в 30 символов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPasswordReg2.Focus();
				}
				else if (tboxPasswordReg.Text != tboxPasswordReg2.Text)
                {
                    MessageBox.Show("Кажется, что один из введённых паролей не совпадают.", "Ошибка #27", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tboxPasswordReg2.Focus();
                }
                else
                {
                    await CheckReg.DisposeAsync();
                    cmd.CommandText = $"SELECT NickName FROM Users WHERE NickName='{tboxLoginReg.Text}'";
                    MySqlDataReader sdr = cmd.ExecuteReader();
                    if (await sdr.ReadAsync())
                    {
                        MessageBox.Show("Кажется, что пользователь с таким логином уже зарегистрирован!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tboxLoginReg.Focus();
                    }
                    else
                    {
                        await sdr.DisposeAsync();
                        cmd.CommandText = $"INSERT INTO Users(NickName, Pass) VALUES('{tboxLoginReg.Text}', " +
                            $"'{tboxPasswordReg.Text}')";
                        cmd.ExecuteNonQuery();

                        MessageBox.Show($"Вы зарегистрировались! Далее Вам нужно будет заново ввести пароль для авторизации.", "Успешная регистрация", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        tabControl.SelectTab(tabAuth);
                        tboxLogin.Text = tboxLoginReg.Text;
                        tboxPassword.Focus();
                        tboxLoginReg.Text = "";
                        tboxPasswordReg.Text = "";
                        tboxPasswordReg2.Text = "";
                    }
                }
                await conn.CloseAsync();
            }
            catch
            { MessageBox.Show("Кажется, Вы вводите некорректные данные или у Вас отсутствует подключение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { await conn.CloseAsync(); }
            await conn.CloseAsync();
        }

        private async void btnAuth_Click(object sender, EventArgs e) // +
        {
            try
            {
				if (tboxLogin.Text == "")
				{
					MessageBox.Show("Кажется, что Вы оставили пустым поле введения логина.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxLogin.Focus();
				}
				else if (!Regex.IsMatch(tboxLogin.Text, @"^[a-zA-Z0-9_]+$"))
				{
					MessageBox.Show("Кажется, что Вы вводите некорректные данные в поле введения логина.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxLogin.Focus();
				}
				else if (tboxPassword.Text == "")
				{
					MessageBox.Show("Кажется, что Вы оставили пустым поле введения пароля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPassword.Focus();
				}
				else if (!Regex.IsMatch(tboxPassword.Text, @"^[a-zA-Z0-9_]+$"))
				{
					MessageBox.Show("Кажется, что Вы вводите некорректные данные в поле введения пароля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					tboxPassword.Focus();
				}
				else
                {
					await conn.OpenAsync();

                    cmd.CommandText = $"SELECT Pass FROM Users WHERE NickName='{tboxLogin.Text}'";
					MySqlDataReader CheckPassword = (MySqlDataReader)await cmd.ExecuteReaderAsync();
					if (await CheckPassword.ReadAsync()) password = CheckPassword[0].ToString();
					await CheckPassword.DisposeAsync();

					cmd.CommandText = $"SELECT NickName FROM Users WHERE NickName='{tboxLogin.Text}' and Pass='{tboxPassword.Text}'";
                    MySqlDataReader sdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();
					if (user == "guest" && await sdr.ReadAsync() && password.ToString() == tboxPassword.Text)
                    {
						await sdr.DisposeAsync();
                        cmd.CommandText = $"SELECT NickName FROM Users WHERE NickName='{tboxLogin.Text}' and Pass='{tboxPassword.Text}'";
                        MySqlDataReader adr = (MySqlDataReader)await cmd.ExecuteReaderAsync();
						if (await adr.ReadAsync()) user = adr[0].ToString();
						await adr.DisposeAsync();
                        labelUser.Visible = false;
                        btnExit.Enabled = true;
                        tabControl.TabPages.Add(tabReview);
                        tabControl.TabPages.Add(tabBooking);
                        tabControl.TabPages.Add(tabMyApplications);
                        tabControl.TabPages.Remove(tabAuth);
                        tabControl.TabPages.Remove(tabReg);

						Settings.Default["username"] = user;
                        Settings.Default.Save();

                        if (user == "uniin" || user == "demo")
                        {
                            moder.Visible = true;
                            moder.Enabled = false;
                            moder.Checked = true;
                        }
                        else
                        {
                            tabControl.SelectTab(tabBooking);
                            tboxComment.Focus();
                        }

                        cmd.CommandText = $"SELECT Id FROM Application WHERE nickname='{user}'";
                        MySqlDataReader CheckSdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                        while (await CheckSdr.ReadAsync())
                        { cboxMyApplications.Items.Add(CheckSdr[0]); }
                        await conn.CloseAsync();
                    }
                    else
                    {
                        MessageBox.Show("Проверьте правильность введённых данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (tboxLogin.Text == "")
                        { tboxLogin.Focus(); }
                        else
                        { tboxPassword.Focus(); }
                    }
					conn.Close();
                    tboxPassword.Text = "";
                }
            }
            catch
            { MessageBox.Show("Кажется, Вы вводите некорректные данные или у Вас отсутствует подключение.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { await conn.CloseAsync(); }
            await conn.CloseAsync();
            await Task.WhenAll();
		}

        private void btnExit_Click(object sender, EventArgs e)
        {
            Connection();
            tabControl.TabPages.Remove(tabReview);
            tabControl.TabPages.Remove(tabBooking);
            tabControl.TabPages.Remove(tabMyApplications);
			cboxMyApplications.Items.Clear();
            cboxMyApplications.Text = "";
			cboxMyApplications.DropDownStyle = ComboBoxStyle.DropDownList;
			cboxMyApplications.DropDownStyle = ComboBoxStyle.DropDown;
			tabControl.TabPages.Add(tabAuth);
            tabControl.TabPages.Add(tabReg);
			//tabControl.TabPages.Add(tabRecovery);
			labelUser.Visible = true;
            user = "guest";
            btnExit.Enabled = false;
			moder.Checked = false;
			moder.Enabled = false;
			tabControl.TabPages.Remove(tabApplications);
			tabControl.TabPages.Remove(tabData);
            tboxPassword.Focus();

            // Очистка: [Мои заявки]
			cboxMyWIFI.Checked = false;
			cboxMyXolod.Checked = false;
			cboxMyFree.Checked = false;
			cboxMyCondi.Checked = false;
			cboxMyDush.Checked = false;
			cboxMyTV.Checked = false;
			myrb2.Checked = false;
			myrb.Checked = false;
			lblMyStatus.Text = "Неизвестно";
            tboxMyComment.Text = "";
            tboxCommentMyModer.Text = "";

            // Очистка: [Обзор]
			cboxWifiR.Checked = false;
			cboxXolodR.Checked = false;
			cboxZavtrak.Checked = false;
			cboxCondiR.Checked = false;
			cboxDushR.Checked = false;
			cboxTvR.Checked = false;
			rbed2.Checked = false;
			rbed1.Checked = false;
			lblStatusR.Text = "Неизвестно";
            tboxSearch.Text = "";
            tBoxCommentR.Text = "";

            // [Очистка комментариев] 

			if (tBoxCommentR.Text == "")
			{ tBoxCommentR.Enabled = false; }
			else { tBoxCommentR.Enabled = true; }

			if (tboxMyComment.Text == "")
			{ tboxMyComment.Enabled = false; }
			else { tboxMyComment.Enabled = true; }

			if (tboxCommentMyModer.Text == "")
			{ tboxCommentMyModer.Enabled = false; }
			else { tboxCommentMyModer.Enabled = true; }

			if (tboxCommentM.Text == "")
			{ tboxCommentM.Enabled = false; }
			else { tboxCommentM.Enabled = true; }

            // Очистка: [Новая заявка]

			tboxComment.Text = "";
			rb2.Checked = false;
			rb1.Checked = true;

			cboxWifi.Checked = false;
			cboxXolod.Checked = false;
			cboxCondi.Checked = false;
			cboxFreer.Checked = false;
			cboxTv.Checked = false;
			cboxDush.Checked = false;

            // Очистка: [Управление заявками]

			label27.Enabled = false;
			lblStatusM.Enabled = false;
			label28.Enabled = false;
			btnStatus0.Enabled = false;
			btnStatus1.Enabled = false;
			btnStatus2.Enabled = false;
			btnStatus3.Enabled = false;

			label5.Enabled = false;
			checkBox1.Enabled = false;
			mCommentEdit.Enabled = false;
			tboxCommentM.ReadOnly = true;
			tboxCommentM.Enabled = false;

			tBoxM.Enabled = true;
            tBoxM.Text = "";
            lblStatusM.Text = "Неизвестно";
			tBoxM.Focus();

            tboxCommentM.Text = "";

			if (tboxCommentM.Text == "")
			{ tboxCommentM.Enabled = false; }
			else { tboxCommentM.Enabled = true; }
			btnUpdateA.Enabled = true;
		}

		private void btnSelectRec_Click(object sender, EventArgs e)
		{ tabControl.SelectTab(tabRecovery); }

        private async void book_Click(object sender, EventArgs e)
        {
            Connection();

			if (string.IsNullOrWhiteSpace(tboxComment.Text))
            { MessageBox.Show($"Кажется, что Вы не заполнили поле комментария!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); tboxComment.Focus(); }
            else if(tboxComment.TextLength > 900)
            { MessageBox.Show($"Комментарий превышает лимит слов: уменьшите его для отправки!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); tboxComment.Focus(); }
            else
            {
                int wifi;
                int xolod;
                int freer;
                int condi;
                int dush;
                int tv;
                int rb;

                if (cboxWifi.Checked) { wifi = 1; }
                else { wifi = 0; }

                if (cboxXolod.Checked) { xolod = 1; }
                else { xolod = 0; }

                if (cboxFreer.Checked) { freer = 1; }
                else { freer = 0; }

                if (cboxCondi.Checked) { condi = 1; }
                else { condi = 0; }

                if (cboxDush.Checked) { dush = 1; }
                else { dush = 0; }

                if (cboxTv.Checked) { tv = 1; }
                else { tv = 0; }

                if (rb1.Checked) { rb = 1; }
                else if (rb2.Checked) { rb = 2; }
                else { rb = 0; }

                await conn.OpenAsync();

                DateTime dt = this.dateTimePicker1.Value.Date;
				cmd.CommandText = $"INSERT INTO Application (title, comment, date, `before`, result, mcomment, tv, wifi, fridge, shower, conditioner, free_breakfast, bed_type, status, nickname) VALUES('Новая заявка', ?comment, CURRENT_TIMESTAMP(), CURRENT_TIMESTAMP(), 0, '', {tv}, {wifi}, {xolod}, {dush}, {condi}, {freer}, '{rb}', 0, '{user}')";
				cmd.Parameters.Add(new MySqlParameter("comment", tboxComment.Text));
				await cmd.ExecuteNonQueryAsync();

				cmd.CommandText = $"SELECT MAX(Id) FROM Application";
				MySqlDataReader adr = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                if (await adr.ReadAsync()) app_id = adr[0].ToString();

				await adr.DisposeAsync();

                int num = Convert.ToInt32(app_id);
                MessageBox.Show($"Заявка #{num} успешно отправлена модераторам!", "Успешная отправка заявки", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cboxMyApplications.Items.Clear();

				cmd.CommandText = $"SELECT Id FROM Application WHERE nickname='{user}'";
				MySqlDataReader CheckSdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();
				while (await CheckSdr.ReadAsync())
                { cboxMyApplications.Items.Add(CheckSdr[0]); }
				await CheckSdr.DisposeAsync();

                tboxComment.Text = "";
                rb2.Checked = false;
                rb1.Checked = true;

                cboxWifi.Checked = false;
                cboxXolod.Checked = false;
                cboxCondi.Checked = false;
                cboxFreer.Checked = false;
                cboxTv.Checked = false;
                cboxDush.Checked = false;
            }
            await conn.CloseAsync();
            await Task.WhenAll();
		}

        private async void btnUpdateR_Click(object sender, EventArgs e)
        {
			Connection();
            if (int.TryParse(tboxSearch.Text, out _))
            {
				await conn.OpenAsync();
				cmd.CommandText = $"SELECT nickname FROM Application WHERE Id LIKE '{tboxSearch.Text}'";
                MySqlDataReader CheckSdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                if(CheckSdr.HasRows)
                {
                    await CheckSdr.DisposeAsync();
                    cmd.CommandText = $"SELECT wifi, fridge, free_breakfast, conditioner, shower, tv, bed_type, status, comment FROM Application WHERE Id='{tboxSearch.Text}'";
                    MySqlDataReader adr = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                    if (await adr.ReadAsync())
                    {
                        wifi = adr.GetValue(0);
                        fridge = adr.GetValue(1);
                        free_breakfast = adr.GetValue(2);
                        conditioner = adr.GetValue(3);
                        shower = adr.GetValue(4);
                        tv = adr.GetValue(5);
                        bed_type = adr.GetValue(6);
                        status = adr.GetValue(7);
                        comment = adr.GetValue(8);

                        if (Convert.ToUInt32(wifi) == 1)
                        { cboxWifiR.Checked = true; }
                        else
                        { cboxWifiR.Checked = false; }

                        if (Convert.ToUInt32(fridge) == 1)
                        { cboxXolodR.Checked = true; }
                        else
                        { cboxXolodR.Checked = false; }

                        if (Convert.ToUInt32(free_breakfast) == 1)
                        { cboxZavtrak.Checked = true; }
                        else
                        { cboxZavtrak.Checked = false; }

                        if (Convert.ToUInt32(conditioner) == 1)
                        { cboxCondiR.Checked = true; }
                        else
                        { cboxCondiR.Checked = false; }

                        if (Convert.ToUInt32(shower) == 1)
                        { cboxDushR.Checked = true; }
                        else
                        { cboxDushR.Checked = false; }

                        if (Convert.ToUInt32(tv) == 1)
                        { cboxTvR.Checked = true; }
                        else
                        { cboxTvR.Checked = false; }

                        if (Convert.ToUInt32(bed_type) == 1)
                        { rbed1.Checked = true; rbed2.Checked = false; }
                        else if (Convert.ToUInt32(bed_type) == 2)
                        { rbed2.Checked = true; rbed1.Checked = false; }
                        else
                        { rbed2.Checked = false; rbed1.Checked = false; }

                        if (Convert.ToUInt32(status) == 0)
                        { lblStatusR.Text = "Открыта"; }
                        else if (Convert.ToUInt32(status) == 1)
                        { lblStatusR.Text = "На рассмотрении"; }
                        else if (Convert.ToUInt32(status) == 2)
                        { lblStatusR.Text = "Требует корректировки"; }
                        else if (Convert.ToUInt32(status) == 3)
                        { lblStatusR.Text = "Отклонена"; }
                        else
                        { lblStatusR.Text = "Неизвестно"; }

                        tBoxCommentR.Text = comment.ToString();
                    }
                    await adr.DisposeAsync();

                    if (tBoxCommentR.Text == "")
                    { tBoxCommentR.Enabled = false; }
                    else { tBoxCommentR.Enabled = true; }

                    await conn.CloseAsync();
                }
                else
                { 
                    MessageBox.Show("Возможно, такой заявки не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                    tboxSearch.Focus();
                }
                await conn.CloseAsync();
            }
            else
            {
                MessageBox.Show("Проверьте правильность введенных данных! \nВозможно, Вы не указали ID заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tboxSearch.Focus();
				await conn.CloseAsync();
			}
		}

        private void btnSelectReg_Click(object sender, EventArgs e)
        { tabControl.SelectTab(tabReg); }

		private async void btnUpdateA_Click(object sender, EventArgs e)
        {
            Connection();
            if (int.TryParse(tBoxM.Text, out _))
            {
                await conn.OpenAsync();
                cmd.CommandText = $"SELECT * FROM Application WHERE Id LIKE '{tBoxM.Text}'";
                MySqlDataReader ACheckSdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                if(ACheckSdr.HasRows)
                {
                    await ACheckSdr.DisposeAsync();
                    cmd.CommandText = $"SELECT status, mcomment FROM Application WHERE Id LIKE '{tBoxM.Text}'";
                    MySqlDataReader bdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                    await bdr.ReadAsync();

                    status = bdr.GetValue(0);
                    m_comment = bdr.GetValue(1);

                    await bdr.DisposeAsync();

                    if (Convert.ToUInt32(status) == 0)
                    { lblStatusM.Text = "Открыта"; }
                    else if (Convert.ToUInt32(status) == 1)
                    { lblStatusM.Text = "На рассмотрении"; }
                    else if (Convert.ToUInt32(status) == 2)
                    { lblStatusM.Text = "Требует корректировки"; }
                    else if (Convert.ToUInt32(status) == 3)
                    { lblStatusM.Text = "Отклонена"; }
                    else
                    { lblStatusM.Text = "Неизвестно"; }

                    tboxCommentM.Text = m_comment.ToString();

                    label27.Enabled = true;
                    lblStatusM.Enabled = true;
                    label28.Enabled = true;
                    btnStatus0.Enabled = true;
                    btnStatus1.Enabled = true;
                    btnStatus2.Enabled = true;
                    btnStatus3.Enabled = true;

                    label5.Enabled = true;
                    checkBox1.Enabled = true;
                    mCommentEdit.Enabled = true;
                    tboxCommentM.ReadOnly = false;
                    tboxCommentM.Enabled = true;

                    tBoxM.Enabled = false;

                    btnUpdateA.Enabled = false;
                    tboxCommentM.Focus();
                }
                else
                {
                    label27.Enabled = false;
                    lblStatusM.Enabled = false;
                    label28.Enabled = false;
                    btnStatus0.Enabled = false;
                    btnStatus1.Enabled = false;
                    btnStatus2.Enabled = false;
                    btnStatus3.Enabled = false;

                    label5.Enabled = false;
                    checkBox1.Enabled = false;
                    mCommentEdit.Enabled = false;
                    tboxCommentM.ReadOnly = true;
                    tboxCommentM.Enabled = false;
                    MessageBox.Show("Возможно, такой заявки не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tBoxM.Focus();
					btnUpdateA.Enabled = true;
				}
                await conn.CloseAsync();
            }
            else
            {
                label27.Enabled = false;
                lblStatusM.Enabled = false;
                label28.Enabled = false;
                btnStatus0.Enabled = false;
                btnStatus1.Enabled = false;
                btnStatus2.Enabled = false;
                btnStatus3.Enabled = false;

                label5.Enabled = false;
                checkBox1.Enabled = false;
                mCommentEdit.Enabled = false;
                tboxCommentM.ReadOnly = true;
                tboxCommentM.Enabled = false;
                MessageBox.Show("Проверьте правильность введенных данных! \nВозможно, Вы не указали ID заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tBoxM.Focus();
				btnUpdateA.Enabled = true;
			}
            await conn.CloseAsync();
        }

        public void ASettings()
        {
            label27.Enabled = false;
            lblStatusM.Enabled = false;
            label28.Enabled = false;
            btnStatus0.Enabled = false;
            btnStatus1.Enabled = false;
            btnStatus2.Enabled = false;
            btnStatus3.Enabled = false;

            label5.Enabled = false;
            checkBox1.Enabled = false;
            mCommentEdit.Enabled = false;
            tboxCommentM.ReadOnly = true;
            tboxCommentM.Enabled = false;
        }
        private async void button10_Click(object sender, EventArgs e)
        {
            Connection();
            await conn.OpenAsync();
            cmd.CommandText = $"UPDATE Application SET status = '0' WHERE Id='{tBoxM.Text}'";
            await cmd.ExecuteNonQueryAsync();

            ASettings();

            cmd.CommandText = $"SELECT status FROM Application WHERE Id='{tBoxM.Text}'";
            MySqlDataReader bdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            await bdr.ReadAsync();
            status = bdr.GetValue(0);
            await bdr.DisposeAsync();

            if (Convert.ToUInt32(status) == 0)
            { lblStatusM.Text = "Открыта"; }
            else if (Convert.ToUInt32(status) == 1)
            { lblStatusM.Text = "На рассмотрении"; }
            else if (Convert.ToUInt32(status) == 2)
            { lblStatusM.Text = "Требует корректировки"; }
            else if (Convert.ToUInt32(status) == 3)
            { lblStatusM.Text = "Отклонена"; }
            else
            { lblStatusM.Text = "Неизвестно"; }

            tBoxM.Enabled = true;
            tBoxM.Focus();
            await conn.CloseAsync();
            btnUpdateA.Enabled = true;
		}

		private async void button5_Click(object sender, EventArgs e)
        {
            Connection();
            await conn.OpenAsync();
            cmd.CommandText = $"UPDATE Application SET status = '1' WHERE Id='{tBoxM.Text}'";
            cmd.ExecuteNonQuery();

            ASettings();

            cmd.CommandText = $"SELECT status FROM Application WHERE Id='{tBoxM.Text}'";
            MySqlDataReader bdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            await bdr.ReadAsync();
            status = bdr.GetValue(0);
            await bdr.DisposeAsync();

            if (Convert.ToUInt32(status) == 0)
            { lblStatusM.Text = "Открыта"; }
            else if (Convert.ToUInt32(status) == 1)
            { lblStatusM.Text = "На рассмотрении"; }
            else if (Convert.ToUInt32(status) == 2)
            { lblStatusM.Text = "Требует корректировки"; }
            else if (Convert.ToUInt32(status) == 3)
            { lblStatusM.Text = "Отклонена"; }
            else
            { lblStatusM.Text = "Неизвестно"; }

            tBoxM.Enabled = true;
            tBoxM.Focus();
            await conn.CloseAsync();
            btnUpdateA.Enabled = true;
		}

        private async void button7_Click(object sender, EventArgs e)
        {
            Connection();
            await conn.OpenAsync();
            cmd.CommandText = $"UPDATE Application SET status = '2' WHERE Id='{tBoxM.Text}'";
            await cmd.ExecuteNonQueryAsync();

            ASettings();

            cmd.CommandText = $"SELECT status FROM Application WHERE Id='{tBoxM.Text}'";
            MySqlDataReader bdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            await bdr.ReadAsync();
            status = bdr.GetValue(0);
            await bdr.DisposeAsync();

            if (Convert.ToUInt32(status) == 0)
            { lblStatusM.Text = "Открыта"; }
            else if (Convert.ToUInt32(status) == 1)
            { lblStatusM.Text = "На рассмотрении"; }
            else if (Convert.ToUInt32(status) == 2)
            { lblStatusM.Text = "Требует корректировки"; }
            else if (Convert.ToUInt32(status) == 3)
            { lblStatusM.Text = "Отклонена"; }
            else
            { lblStatusM.Text = "Неизвестно"; }

            tBoxM.Enabled = true;
            tBoxM.Focus();
            await conn.CloseAsync();
			btnUpdateA.Enabled = true;
		}

        private async void button6_Click(object sender, EventArgs e)
        {
            Connection();
            await conn.OpenAsync();
            cmd.CommandText = $"UPDATE Application SET status = '3' WHERE Id='{tBoxM.Text}'";
            await cmd.ExecuteNonQueryAsync();

            ASettings();

            cmd.CommandText = $"SELECT status FROM Application WHERE Id='{tBoxM.Text}'";
            MySqlDataReader bdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

            await bdr.ReadAsync();
            status = bdr.GetValue(0);
            await bdr.DisposeAsync();

            if (Convert.ToUInt32(status) == 0)
            { lblStatusM.Text = "Открыта"; }
            else if (Convert.ToUInt32(status) == 1)
            { lblStatusM.Text = "На рассмотрении"; }
            else if (Convert.ToUInt32(status) == 2)
            { lblStatusM.Text = "Требует корректировки"; }
            else if (Convert.ToUInt32(status) == 3)
            { lblStatusM.Text = "Отклонена"; }
            else
            { lblStatusM.Text = "Неизвестно"; }

            tBoxM.Enabled = true;
            tBoxM.Focus();
            await conn.CloseAsync();
			btnUpdateA.Enabled = true;
		}

        private async void mCommentEdit_Click(object sender, EventArgs e)
        {
            Connection();
            if (tboxCommentM.TextLength > 900)
            {
                MessageBox.Show($"Комментарий превышает лимит слов: уменьшите его для отправки!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                ASettings();

                tBoxM.Enabled = true;
                tBoxM.Focus();
				btnUpdateA.Enabled = true;
			}
            else
            {
                await conn.OpenAsync();
                cmd.CommandText = $"UPDATE Application SET mcomment = '{tboxCommentM.Text}' WHERE Id='{tBoxM.Text}'";
                await cmd.ExecuteNonQueryAsync();
                await conn.CloseAsync();

                MessageBox.Show($"Вы успешно сменили комментарий модератора последней загруженной заявки.", "Смена комментария", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ASettings();

                tBoxM.Enabled = true;
                tBoxM.Focus();

				if (tboxCommentM.Text == "")
				{ tboxCommentM.Enabled = false; }
				else { tboxCommentM.Enabled = true; }
                btnUpdateA.Enabled = true;
			}
            await conn.CloseAsync();
        }

        private void tboxComment_TextChanged(object sender, EventArgs e)
        {
            labelLimit.Text = tboxComment.Text.Replace("\r", "").Length.ToString() + "/900";

            if (tboxComment.TextLength > 900)
            { labelLimit.ForeColor = Color.Red; }
            else
            { labelLimit.ForeColor = Color.Black; }
        }

        private void checkHidePasswordAuth_CheckedChanged(object sender, EventArgs e)
        {
            if(checkHidePasswordAuth.Checked)
            { tboxPassword.PasswordChar = '*'; }
            else
            { tboxPassword.PasswordChar = '\0'; }
        }

        private void checkHidePasswordReg_CheckedChanged(object sender, EventArgs e)
        {
            if (checkHidePasswordReg.Checked)
            {
                tboxPasswordReg.PasswordChar = '*';
                tboxPasswordReg2.PasswordChar = '*';
            }
            else
            {
                tboxPasswordReg.PasswordChar = '\0';
                tboxPasswordReg2.PasswordChar = '\0';
            }
        }

        private async void MyApplicationsUpdate_Click(object sender, EventArgs e)
        {
            Connection();
            if (int.TryParse(cboxMyApplications.Text, out _))
            {
				await conn.OpenAsync();
				cmd.CommandText = $"SELECT nickname FROM Application WHERE Id LIKE '{cboxMyApplications.Text}'";
				MySqlDataReader CheckSdr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                if (CheckSdr.HasRows)
                {
                    await CheckSdr.ReadAsync();
                    app_id = CheckSdr.GetValue(0);

                    if (app_id.ToString() == user)
                    {
                        await CheckSdr.DisposeAsync();

                        cmd.CommandText = $"SELECT wifi, fridge, free_breakfast, conditioner, shower, tv, bed_type, status, comment, mcomment FROM Application WHERE Id='{cboxMyApplications.Text}'";
                        MySqlDataReader adr = (MySqlDataReader)await cmd.ExecuteReaderAsync();

                        await adr.ReadAsync();

                        wifi = adr.GetValue(0);
                        fridge = adr.GetValue(1);
                        free_breakfast = adr.GetValue(2);
                        conditioner = adr.GetValue(3);
                        shower = adr.GetValue(4);
                        tv = adr.GetValue(5);
                        bed_type = adr.GetValue(6);
                        status = adr.GetValue(7);
                        comment = adr.GetValue(8);
                        m_comment = adr.GetValue(9);

                        await adr.DisposeAsync();

                        if (Convert.ToUInt32(wifi) == 1)
                        { cboxMyWIFI.Checked = true; }
                        else
                        { cboxMyWIFI.Checked = false; }

                        if (Convert.ToUInt32(fridge) == 1)
                        { cboxMyXolod.Checked = true; }
                        else
                        { cboxMyXolod.Checked = false; }

                        if (Convert.ToUInt32(free_breakfast) == 1)
                        { cboxMyFree.Checked = true; }
                        else
                        { cboxMyFree.Checked = false; }

                        if (Convert.ToUInt32(conditioner) == 1)
                        { cboxMyCondi.Checked = true; }
                        else
                        { cboxMyCondi.Checked = false; }

                        if (Convert.ToUInt32(shower) == 1)
                        { cboxMyDush.Checked = true; }
                        else
                        { cboxMyDush.Checked = false; }

                        if (Convert.ToUInt32(tv) == 1)
                        { cboxMyTV.Checked = true; }
                        else
                        { cboxMyTV.Checked = false; }

                        if (Convert.ToUInt32(bed_type) == 1)
                        { myrb.Checked = true; myrb2.Checked = false; }
                        else if (Convert.ToUInt32(bed_type) == 2)
                        { myrb2.Checked = true; myrb.Checked = false; }
                        else
                        { myrb2.Checked = false; myrb.Checked = false; }

                        if (Convert.ToUInt32(status) == 0)
                        { lblMyStatus.Text = "Открыта"; }
                        else if (Convert.ToUInt32(status) == 1)
                        { lblMyStatus.Text = "На рассмотрении"; }
                        else if (Convert.ToUInt32(status) == 2)
                        { lblMyStatus.Text = "Требует корректировки"; }
                        else if (Convert.ToUInt32(status) == 3)
                        { lblMyStatus.Text = "Отклонена"; }
                        else
                        { lblMyStatus.Text = "Неизвестно"; }

                        tboxMyComment.Text = comment.ToString();
                        tboxCommentMyModer.Text = m_comment.ToString();

                        if (tboxMyComment.Text == "")
                        { tboxMyComment.Enabled = false; }
                        else { tboxMyComment.Enabled = true; }

                        if (tboxCommentMyModer.Text == "")
                        { tboxCommentMyModer.Enabled = false; }
                        else { tboxCommentMyModer.Enabled = true; }
                    }
					else
					{
						MessageBox.Show("Возможно, эта заявка создана не Вами.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
						cboxMyApplications.Focus();
					}
					await conn.CloseAsync();
				}
                else
                { 
                    MessageBox.Show("Возможно, такой заявки не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboxMyApplications.Focus();
                }
				await conn.CloseAsync();
            }
            else
            {
                MessageBox.Show("Проверьте правильность введенных данных! \nВозможно, Вы не указали ID заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboxMyApplications.Focus();
                await conn.CloseAsync();
            }
            await conn.CloseAsync();
        }
	}
}