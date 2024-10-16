import unittest
import httpx


# 5 test integrations happy path
# 4 test integrations edge cases
class TestLocation(unittest.TestCase):

    def setUp(self):
        self.admin_token = "a1b2c3d4e5"
        self.reader_token = "f6g7h8i9j0"
        self.headers = {"Content-Type": "application/json", "API_KEY": self.admin_token}
        self.client = httpx.Client(base_url="http://localhost:3000/api/v1", headers=self.headers)
    
    # Returns an 500 error
    def test_get_location_not_found(self):
        response = self.client.get("/locations/-1")
        self.assertEqual(response.status_code, 404)

    # Returns an 500 error
    def test_update_location_not_found(self):
        updated_location = {"name": "Updated Location", "address": "456 Main St"}
        response = self.client.put("/locations/-1", json=updated_location)
        self.assertEqual(response.status_code, 404)

    # Returns an 500 error
    def test_delete_location_not_found(self):
        response = self.client.delete("/locations/-1")
        self.assertEqual(response.status_code, 404)
    
    # Edge case: Test sending a malformed JSON body (400 Bad Request).
    def test_create_with_malformed_json(self):
        malformed_json = '{"name": "test location", "address": "123 straat"'
        response = self.client.post("/locations", data=malformed_json)
        self.assertEqual(response.status_code, httpx.codes.BAD_REQUEST) 

    def test_get_location_by_id(self):        
        response = self.client.get("/locations/1")
        self.assertEqual(response.status_code, httpx.codes.OK)
        self.assertIn("id", response.json())

    def test_delete_location(self):       
        response = self.client.delete("/locations/115588")
        self.assertEqual(response.status_code, httpx.codes.OK)

    def test_update_location(self):        
        updated_location = {"name": "Updated Location"}
        response = self.client.put("/locations/115588", json=updated_location)
        self.assertEqual(response.status_code, httpx.codes.OK)
        
    def test_create_location(self):        
        new_location = {
            "id": 115588,
            "warehouse_id": 8,
            "code": "MYTEST",
            "name": "TEST LOCATION",
            "created_at": "2024-10-15 10:21:32",
            "updated_at": "2024-10-15 10:21:32",
        }
        response = self.client.post("/locations", json=new_location)
        self.assertEqual(response.status_code, httpx.codes.CREATED)

    def test_get_all_locations(self):
        response = self.client.get("/locations")
        self.assertEqual(response.status_code, httpx.codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)
        
if __name__ == "__main__":
    unittest.main()
