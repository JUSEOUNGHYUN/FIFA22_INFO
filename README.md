# 📛 FIFA22_INFO

📍 프로젝트 기간 : 2023.07.10 ~ 2023.10.01 (3개월)
- 제가 평소에 즐겨하던 게임 FIFA22 감독모드를 플레이 하던 도중 시즌마다 각 리그 우승팀과 컵 대회 우승팀을 보기 좋게 관리해보자는 생각에 회사에서 배웠던 WPF기술로 감독 모드 각 리그 시뮬레이션결과(프리미어리그, 챔피언스리그, 유로파리그 등등)를 PostgreSQL DB에 저장하고 그 데이터들을 WPF DataGrid에 보기 좋게 출력하는 프로그램을 구현했습니다.

# 📌 개요


# 🛠️ 기술 및 도구
<img src="https://img.shields.io/badge/C Sharp-239120?style=flat-square&logo=C Sharp&logoColor=white"/> <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=flat-square&logo=PostgreSQL&logoColor=white"/> <img src="https://img.shields.io/badge/WPF-40AEF0?style=flat-square&logo=WPF&logoColor=white"/>

# 🎏 기능 구현

### 0. 기본 화면
📍 화면 구성
1. 
![기본화면](https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/47a70c2c-c0cb-414d-aedb-6b2abf5c853a)


### 1. DataGrid Data Binding (프리미어리그 조회 UI)
![프리미어리그 UI](https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/a8a846d9-69e4-43ae-855e-ce95c09c5fb8)

    // Create DataBinding Class (in .cs)
    public class Other_League
    {
        public string League_Year {  get; set; }
        public string ChampionsLOGO { get; set; }
        public string Champions { get; set; }
        public string Second_PlaceLOGO { get; set; }
        public string Second_Place { get; set; }
        public string Third_PlaceLOGO { get; set; }
        public string Third_Place { get; set; }
        public string Fourth_PlaceLOGO { get; set; }
        public string Fourth_Place { get; set; }
        public string Remark { get; set; }
    }

    // xaml DataBinding
    <DataGridTextColumn Header="YEAR" Binding="{Binding Path=League_Year}" Width="80" IsReadOnly="True">

### 2. 특정 팀 우승(준우승)년도 
![특정팀 우승년도 조회](https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/02eab86d-eca1-43cb-836c-df469caf8d61)

### 3. DataGridTemplateColumn.CellEditingTemplate
- 조회된 DataGrid에서 Remark를 UI에서 직접 수정 가능하고 수정된 데이터가 DB에 저장되는 기능 구현

https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/1e13cdbe-c920-4efb-83c5-f98801a2967c

    <DataGridTemplateColumn.CellTemplate>
      <DataTemplate>
        <TextBlock Text="{Binding Remark}" TextWrapping="Wrap" 
                 Background="Transparent" 
                 VerticalAlignment="Center"                                                                                           
                   FontSize="12"
                 />
      </DataTemplate>
    </DataGridTemplateColumn.CellTemplate>
    <DataGridTemplateColumn.CellEditingTemplate>
        <DataTemplate>
            <TextBox Text="{Binding Remark}" TextWrapping="Wrap" BorderThickness="0" VerticalContentAlignment="Center" VerticalAlignment="Center" Background="Transparent" />
        </DataTemplate>
    </DataGridTemplateColumn.CellEditingTemplate>

### 4. TreeView (Select All Team)
https://github.com/JUSEOUNGHYUN/FIFA22_INFO/assets/80812790/7160bac3-ee3a-438f-a566-4791ba1c500b

    private void Treeview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      System.Diagnostics.Trace.WriteLine(" ComeIn MouseDoubleClick Event");
      TreeViewItem view = sender as TreeViewItem;
      TeamItem item = AllTeam_treeView.SelectedItem as TeamItem;
        try
        {
            if(item != null)
            {
                int n = item.Team_Item.Count;
                if (n == 0)
                {
                    string sTeamName = item.Team_Name.ToString();
                    DataPassProdCd(sTeamName);
                    this.Close();
                }
            }
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
