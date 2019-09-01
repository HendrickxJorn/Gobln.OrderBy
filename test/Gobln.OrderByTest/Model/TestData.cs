using System;

namespace Gobln.OrderByTest.Model
{
    public static class TestData
    {
        public static MoonWalker[] MoonWalkers
        {
            get
            {
                return new MoonWalker[] {
                    new MoonWalker() {
                        Id = 1,
                        LunarEvaDate = new DateTime(1969, 7, 21),
                        FirstName = "Neil",
                        LastName = "Armstrong",
                        Remark = "Died at 82",
                        GuiId = "39fb1e11-fe3c-498d-8059-5b9315cb00b4"
                    },
                    new MoonWalker() {
                        Id = 2,
                        LunarEvaDate = new DateTime(1969, 7, 21),
                        FirstName = "Buzz",
                        LastName = "Aldrin",
                        GuiId="3acd1738-99ea-4c8f-9c7f-959c32b389a7"
                    },
                    new MoonWalker() {
                        Id = 3,
                        LunarEvaDate = new DateTime(1969, 11, 19),
                        FirstName = "Pete",
                        LastName ="Conrad",
                        Remark = "Died at 69",
                        GuiId = "cc72f055-5f85-4a95-9c27-bd57a43af6ff"
                    },
                    new MoonWalker() {
                        Id = 4,
                        LunarEvaDate = new DateTime(1969, 11, 19),
                        FirstName = "Alan",
                        LastName = "Bean",
                        Remark = "Passed away.",
                        GuiId = "17f8610a-7661-433d-b810-87bee49c3fa1"
                    },
                    new MoonWalker() {
                        Id = 5,
                        LunarEvaDate = new DateTime(1971, 2, 5),
                        FirstName = "Alan",
                        LastName = "Shepard",
                        Remark = "Passed away.",
                        GuiId = "89e503aa-8e0b-4b8f-b4ae-00314db4d716"
                    },
                    new MoonWalker() {
                        Id = 6,
                        LunarEvaDate = new DateTime(1971, 2, 5),
                        FirstName = "Edgar",
                        LastName = "Mitchell",
                        Remark = "Died at 85",
                        GuiId = "459d5c40-d1b9-4091-9816-63b09ae5a1e6"
                    },
                    new MoonWalker() {
                        Id = 7,
                        LunarEvaDate = new DateTime(1971, 7, 31),
                        FirstName = "David",
                        LastName = "Scott",
                        Remark = "",
                        GuiId = "e64ac4b9-bb8c-4c9a-92b1-deccfb4c76a1"
                    },
                    new MoonWalker() {
                        Id = 8,
                        LunarEvaDate = new DateTime(1971, 7, 31),
                        FirstName = "James",
                        LastName = "Irwin",
                        Remark = "Died at 61",
                        GuiId ="ebdfc8ec-6429-4ff0-a5e7-a87a0da0320a"
                    },
                    new MoonWalker() {
                        Id = 9,
                        LunarEvaDate = new DateTime(1972, 4, 21),
                        FirstName = "John",
                        LastName = "Young",
                        Remark = "Died at 87",
                        GuiId = "104de8a5-8b99-4198-a403-44a6d190a3d5"
                    },
                    new MoonWalker() {
                        Id = 10,
                        LunarEvaDate = new DateTime(1972, 4, 21),
                        FirstName = "Charles",
                        LastName = "Duke",
                        GuiId ="c7024f80-8d67-4dfc-866b-52be143f7df6"
                    },
                    new MoonWalker() {
                        Id = 11,
                        LunarEvaDate = new DateTime(1972, 12, 11),
                        FirstName = "Gene",
                        LastName = "Cernan",
                        Remark = "Died at 82",
                        GuiId = "29aa6467-4326-48d3-a4ca-50934a840abf"
                    },
                    new MoonWalker() {
                        Id = 12,
                        LunarEvaDate = new DateTime(1972, 12, 11),
                        FirstName = "Harrison",
                        LastName = "Schmitt",
                        GuiId = "6d14e47a-bfc6-40b8-93fa-d1bc3c12ae47"
                    }
                };
            }
        }
    }
}