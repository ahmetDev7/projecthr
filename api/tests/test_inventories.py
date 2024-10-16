import unittest
import httpx


# 5 test integrations happy path
# 4 test integrations edge cases
class TestLocation(unittest.TestCase):

    def setUp(self):
        self.admin_token = "a1b2c3d4e5"
        self.reader_token = "f6g7h8i9j0"
        self.headers = {"Content-Type": "application/json", "API_KEY": self.admin_token}
        self.client = httpx.Client(
            base_url="http://localhost:3000/api/v1", headers=self.headers
        )

    # Returns an 500 error
    def test_get_inventory_not_found(self):
        response = self.client.get("/inventories/-1")
        self.assertEqual(response.status_code, 404)

    # Returns an 500 error
    def test_update_inventory_not_found(self):
        updated_location = {"name": "Updated Location", "address": "456 Main St"}
        response = self.client.put("/inventories/-1", json=updated_location)
        self.assertEqual(response.status_code, 404)

    # Returns an 500 error
    def test_delete_inventory_not_found(self):
        response = self.client.delete("/inventories/-1")
        self.assertEqual(response.status_code, 404)

    # Returns an 500 error
    def test_create_with_malformed_json(self):
        malformed_json = '{"id": 7898189 ,"item_id": "PO984156",'
        response = self.client.post("/inventories", data=malformed_json)
        self.assertEqual(response.status_code, httpx.codes.BAD_REQUEST)

    def test_delete_inventory(self):
        response = self.client.delete("/inventories/888555222")
        self.assertEqual(response.status_code, httpx.codes.OK)

    def test_update_inventory(self):
        updated_location = {"name": "Updated Location"}
        response = self.client.put("/inventories/888555222", json=updated_location)
        self.assertEqual(response.status_code, httpx.codes.OK)

    def test_create_inventory(self):
        new_inventory = {
            "id": 888555222,
            "item_id": "P93741",
            "description": "Integration test",
            "item_reference": "test_sqr622",
            "locations": [1,2],
            "total_on_hand": 100,
            "total_expected": 200,
            "total_ordered": 111,
            "total_allocated": 50,
            "total_available": 200,
            "created_at": "2024-09-19 16:08:24",
            "updated_at": "2024-09-26 06:37:56",
        }
        response = self.client.post("/inventories", json=new_inventory)
        self.assertEqual(response.status_code, httpx.codes.CREATED)
    
    def test_get_inventory_by_id(self):
        response = self.client.get("/inventories/1")
        self.assertEqual(response.status_code, httpx.codes.OK)
        self.assertIn("id", response.json())

    def test_get_all_inventory(self):
        response = self.client.get("/inventories")
        self.assertEqual(response.status_code, httpx.codes.OK)
        self.assertGreaterEqual(len(response.json()), 1)


if __name__ == "__main__":
    unittest.main()
