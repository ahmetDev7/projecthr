import unittest
from httpx import Client
from httpx import codes

# 12 test integrations happy path
# 0 test integrations edge cases
class TestWarehouse(unittest.TestCase):

    def setUp(self):
        self.admin_token = "a1b2c3d4e5"
        self.reader_token = "f6g7h8i9j0"
        self.headers = {"Content-Type": "application/json", "API_KEY": ""}
        self.client = Client(base_url="http://localhost:3000/api/v1", headers=self.headers)


    # NORMAL TEST INTEGRATIONS ADMIN
    # Get All
    def test_get_all_warehouses_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token
        response = self.client.get("/warehouses")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)

    # Get Single by ID
    def test_get_one_by_id_warehouses_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token
        response = self.client.get("/warehouses/28")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)

    # Get Single by ID Locations
    def test_get_one_by_location_warehouses_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token
        response = self.client.get("/warehouses/33/locations")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)

    # Delete By ID
    def test_delete_warehouse_by_id_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token
        response = self.client.delete("/warehouses/89745564")
        self.assertEqual(response.status_code, codes.OK)

    # Update By ID
    def test_update_warehouse_by_id_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token
        updated_data = {
            "id": 89745564,
            "name": "Updated Warehouse",
            "location": "Rotterdam",
            "capacity": 1500
        }
        response = self.client.put(f"/warehouses/89745564", json=updated_data)
        self.assertEqual(response.status_code, codes.OK)

    # Create Warehouse limited data.
    def test_create_warehouse_with_limited_data(self):
        self.client.headers["API_KEY"] = self.admin_token

        minimal_warehouse = {
            "id": 1000001,
            "name": "Minimal Warehouse",
            "location": "Utrecht"
        }

        response = self.client.post("/warehouses", json=minimal_warehouse)
        self.assertEqual(response.status_code, codes.CREATED)        


    #create warehouse with location details
    def test_create_warehouse_with_location_details(self):
        self.client.headers["API_KEY"] = self.admin_token

        detailed_location_warehouse = {
            "id": 1000007,
            "name": "Detailed Location Warehouse",
            "location": "Nijmegen",
            "address": "123 Industrial Zone",
            "zip_code": "6541 AB",
            "capacity": 1500
        }

        response = self.client.post("/warehouses", json=detailed_location_warehouse)
        self.assertEqual(response.status_code, codes.CREATED)        


    #Create Warehouse with large capacity
    def test_create_warehouse_with_large_capacity(self):
        self.client.headers["API_KEY"] = self.admin_token

        large_capacity_warehouse = {
            "id": 1000002,
            "name": "Large Capacity Warehouse",
            "location": "Eindhoven",
            "capacity": 100000
        }

        response = self.client.post("/warehouses", json=large_capacity_warehouse)
        self.assertEqual(response.status_code, codes.CREATED)        


    # Create Warehouse
    def test_create_warehouse_as_admin(self):
        self.client.headers["API_KEY"] = self.admin_token
        warehouse_data = {
            "id": 89745564,
            "name": "New Warehouse",
            "location": "Amsterdam",
            "capacity": 1000
        }

        response = self.client.post("/warehouses", json=warehouse_data)
        self.assertEqual(response.status_code, codes.CREATED)

    #NORMAL TEST INTEGRATIONS READER
    #Get All
    def test_get_all_warehouses_as_reader(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/warehouses")
        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)

    # Get Single by ID
    def test_get_one_by_id_warehouses_as_reader(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/warehouses/1")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)

    # Get Single by ID Locations
    def test_get_one_by_id_location_warehouses_as_reader(self):
        self.client.headers["API_KEY"] = self.reader_token
        response = self.client.get("/warehouses/1/locations")

        self.assertEqual(response.status_code, codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)    

if __name__ == "__main__":
    unittest.main()