import unittest
from httpx import Client
from httpx import codes

# 9 test integrations happy path
# 0 test integrations edge cases
class TestSuppliers(unittest.TestCase):
    
    def setUp(self):
        self.admin_token = "a1b2c3d4e5"
        self.reader_token = "f6g7h8i9j0"
        self.headers = {"Content-Type": "application/json", "API_KEY": ""}
        self.client = Client(base_url="http://localhost:3000/api/v1", headers=self.headers)

    
  
    # Get all
    def test_get_all_suppliers_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token
        response = self.client.get("/suppliers")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)


    # Get Single by ID
    def test_get_supplier_by_id_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token
        response = self.client.get("/suppliers/1")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)


    # Get Single items for specific supplier.
    def test_get_items_for_supplier(self):
        self.client.headers["API_KEY"] = self.admin_token
        response = self.client.get("/suppliers/1/items")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)

    # Delete a specific supplier by ID.
    def test_delete_supplier_by_id_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token        
        response = self.client.delete("/suppliers/9998777333")

        self.assertEqual(response.status_code, codes.OK)

    
    # Update a specific supplier by ID.
    def test_update_supplier_by_id_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token

        updated_supplier = {
            "id": 9998777333,
            "code": "NIFFO-UPDATED",
            "name": "Updated Lee, Parks and Johnson",
            "address": "1234 New Sullivan Street",
            "address_extra": "Suite 100",
            "city": "New Anitaburgh",
            "zip_code": "99999",
            "province": "New Illinois",
            "country": "Germany",
            "contact_name": "Updated Toni Barnett",
            "phonenumber": "+49 123 456 7890",
            "reference": "LPaJ-SUP0002",
            "niffo": "nieuwe gekke niffo straat"
        }

        response = self.client.put("/suppliers/9998777333", json=updated_supplier)
        self.assertEqual(response.status_code, codes.OK)


    # Create a new supplier.
    def test_create_supplier_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token

        new_supplier = {
            "id": 9998777333,
            "code": "NIFFO",
            "name": "Lee, Parks and Johnson",
            "address": "5989 Sullivan Drives",
            "address_extra": "Apt. 996",
            "city": "Port Anitaburgh",
            "zip_code": "91688",
            "province": "Illinois",
            "country": "Czech Republic",
            "contact_name": "Toni Barnett",
            "phonenumber": "363.541.7282x36825",
            "reference": "LPaJ-SUP0001",
            "niffo": "gekke niffo straat"
        }

        response = self.client.post("/suppliers", json=new_supplier)
        self.assertEqual(response.status_code, codes.CREATED)

    #NORMAL TEST INTEGRATIONS READER
    #Get All
    def test_get_all_suppliers_as_reader(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/suppliers")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)
    
    # Get Single by ID
    def test_get_supplier_by_id_as_reader(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/suppliers/1")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)
    
    # Get Single items for specific supplier.
    def test_get_items_for_supplier_as_reader(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/suppliers/1/items")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)


if __name__ == "__main__":
    unittest.main()

