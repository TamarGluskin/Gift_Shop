<template>
  <div>
    <el-input v-model="searchQuery" placeholder="חפש מוצר" @input="searchProduct"></el-input>
    <el-table :data="filteredProducts" style="width: 100%">
      <el-table-column prop="ProductCode" label="קוד מוצר"></el-table-column>
      <el-table-column prop="ProductName" label="שם מוצר"></el-table-column>
      <el-table-column prop="Description" label="תיאור מוצר"></el-table-column>
      <el-table-column prop="StartDate" label="תאריך תחילת מחירה"></el-table-column>
     <el-table-column label="תמונה">
     <template v-slot:default="scope">
      <img :src="'https://localhost:44335/' + scope.row.ImagePath"  alt="תמונה" style="max-width: 50px;" />
       </template>
      </el-table-column>
      <el-table-column label="פעולות">
        <template v-slot:default="scope">
          <el-button @click="editProduct(scope.row)" size="mini">ערוך</el-button>
          <el-button @click="deleteProduct(scope.row.ProductCode); console.log(scope.row) " size="mini" type="danger">מחק</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-dialog v-model="dialogVisible" title="הוספת/עריכת מוצר">
      <el-form :model="form">
        <el-form-item label="קוד מוצר" :label-width="'120px'">
          <el-input v-model="form.ProductCode" placeholder="הכנס קוד מוצר" :disabled="form.ProductCode !== undefined"></el-input>
        </el-form-item>
        <el-form-item label="שם מוצר" :label-width="'120px'">
          <el-input v-model="form.ProductName" placeholder="הכנס שם מוצר"></el-input>
        </el-form-item>
        <el-form-item label="תיאור מוצר" :label-width="'120px'">
          <el-input v-model="form.Description" placeholder="הכנס תיאור מוצר"></el-input>
        </el-form-item>
      <el-form-item label="תמונה" :label-width="'120px'">
  <input type="file" ref="fileInput" @change="handleImageUpload" />
      <img v-if="form.ImagePreview" :src="form.ImagePreview" alt="תצוגה מקדימה" style="max-width: 100%; margin-top: 10px;" />
</el-form-item>
        <el-form-item label="תאריך תחילת מחירה" :label-width="'120px'">
          <el-date-picker v-model="form.StartDate" type="date" placeholder="בחר תאריך"></el-date-picker>
        </el-form-item>
      </el-form>
      <template v-slot:footer>
        <el-button @click="dialogVisible = false">בטל</el-button>
        <el-button type="primary" @click="saveProduct">שמור</el-button>
      </template>
    </el-dialog>
    <el-button type="primary" @click="openDialog">הוסף מוצר חדש</el-button>
  </div>
</template>

<script>
//computed
import { ref,computed, onMounted } from 'vue';
import { ElDialog, ElForm, ElFormItem, ElInput, ElButton, ElTable, ElTableColumn, ElDatePicker } from 'element-plus';
import 'element-plus/dist/index.css';

export default {
  name: 'ProductManagement',
  components: {
    ElDialog,
    ElForm,
    ElFormItem,
    ElInput,
    ElButton,
    ElTable,
    ElTableColumn,
    ElDatePicker,
  },
  setup() {
    const dialogVisible = ref(false);
    const products = ref([]);
    const form = ref({
      ProductCode: '',
      ProductName: '',
      Description: '',
      StartDate: '',
      ImagePath: null,
    });
    const searchQuery = ref('');

    // Fetch products from server
    const fetchProducts = async () => {
      try {
        const response = await fetch('https://localhost:44335/api/products/getall');
        if (!response.ok) throw new Error('Failed to fetch products');
          const data = await response.json(); 
    products.value = data;  
    console.log('Products loaded:', products.value);
  } catch (error) {
    console.error('Failed to load products:', error);
  }
    };
    

    // Save new or updated product
   const saveProduct = async () => {
    try {
        // המרת התמונה לפורמט Base64
        const imageFile =fileInput.value.files[0]; 
        const base64Image = await convertToBase64(imageFile);
         // const fileInputElement = document.querySelector('#fileInput'); 
    //const imageFile = fileInputElement?.files[0]; 
//let base64Image = '';
    //if (imageFile && imageFile instanceof Blob) {
    //  base64Image = await convertToBase64(imageFile); 
    //} else {
     // console.log('No valid image file selected');
   // }
        console.log('ImagePath before sending:', form.value.ImagePath);
        const product = {
            ProductCode: form.value.ProductCode,
            ProductName: form.value.ProductName,
            Description: form.value.Description,
            StartDate:  form.value.StartDate,
            ImagePath: form.value.ImagePath,
            Image: base64Image 
        };

        
       const method = product.ProductCode  ? 'PUT' : 'POST';
    const endpoint = product.ProductCode ? 
      `https://localhost:44335/api/products/update` : 
      `https://localhost:44335/api/products/add`;

       
        console.log('Sending product data to server:', product);
        
        const response = await fetch(endpoint, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(product), // שליחה של המוצר כ-JSON
              
            

        });
        console.log('Server response:', response);

        if (!response.ok) throw new Error('Failed to save product');
        await fetchProducts();
        dialogVisible.value = false;
    } catch (error) {
        console.error('Error saving product:', error);
    }
};

// פונקציה להמיר קובץ לתמונה בפורמט Base64
function convertToBase64(file) {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onloadend = () => {
      const base64String = reader.result.split(',')[1]; 
      console.log('Converted image to Base64:', base64String);
      resolve(base64String);
    };
    reader.onerror = reject;
    reader.readAsDataURL(file); 
  });
}
    
    // Delete a product
    const deleteProduct = async (productCode) => {
      try {
        const response = await fetch(`https://localhost:44335/api/products/delete/${productCode}`,
         { method: 'DELETE'
        
          });
        if (!response.ok) throw new Error('Failed to delete product');
        await fetchProducts();
         this.fetchProducts()
      } catch (error) {
        console.error('Error deleting product:', error);
      }
    };
const filteredProducts = computed(() => {
  return products.value.filter((product) => {
    const productCode = String(product.ProductCode).toLowerCase();
    const productName = String(product.ProductName).toLowerCase();
    const description = String(product.Description || "").toLowerCase();

    const query = searchQuery.value.toLowerCase();

    return productCode.includes(query) || productName.includes(query)||  description.includes(query);
  });
});
    // Filter products based on search query
   // const filteredProducts = computed(() => {
  //return products.value.filter((product) =>
  //typeof product.productCode === "string" &&
  //product.productCode.includes(searchQuery.value)||
   // product.productName.includes(searchQuery.value) 
    
  //);
//});

    // Open dialog for new product
    const openDialog = () => {
      form.value = { ProductCode: '', ProductName: '', Description: '', StartDate: '', ImagePath: null };
      dialogVisible.value = true;
    };

    // Edit existing product
    const editProduct = (product) => {
      form.value = { ...product };
     if (form.value.ImagePath) {
   
    form.value.ImagePreview = `https://localhost:44335/${form.value.ImagePath}`;
  } else {
   
    form.value.ImagePreview = null;
  }
  dialogVisible.value = true;
    };

   
   const fileInput = ref(null); 

    const handleImageUpload = () => {
   if (fileInput.value && fileInput.value.files.length > 0) {
    const file = fileInput.value.files[0];
    convertToBase64(file)
      .then(base64Image => {
       
         form.value.ImagePreview = `data:image/${file.type.split('/')[1]};base64,${base64Image}`; 
       
        form.value.Image = base64Image; 
      })
          .catch(error => console.error('Error uploading image:', error));
      } else {
        console.error('No file selected');
      }
};


    // Fetch products on component mount
    onMounted(fetchProducts);

//filteredProducts,
    return {
      dialogVisible,
      products,
      form,
      filteredProducts,
      searchQuery,
      saveProduct,
      openDialog,
      editProduct,
      deleteProduct,
      handleImageUpload,
      fileInput,
    };
  },
};
</script>


<style scoped>
#app {
  font-family: Arial, sans-serif;
  margin: 20px;
}
.dialog-footer {
  text-align: right;
}
</style>

